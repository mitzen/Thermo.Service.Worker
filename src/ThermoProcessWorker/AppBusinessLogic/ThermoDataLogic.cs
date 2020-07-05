using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Service.ThermoProcessWorker.RestServices;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using Service.ThermoDataModel.Requests;
using Service.MessageBusServiceProvider.CheckPointing;
using System.Collections.Generic;
using Service.MessageBusServiceProvider.Converters;
using Microsoft.Azure.Amqp.Framing;

namespace Service.ThermoProcessWorker.AppBusinessLogic
{
    public class ThermoDataLogic : IThermoDataLogic
    {
        private const string ThermoRestApiConfigurationKey = "ThermoRestConfiguration";
        private const string ServiceWorkerConfigirationKey = "ServiceWorkerConfiguration";
        private const string MessageSpacer = "##############################################################";
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ThermoRestConfiguration _restConfiguration;
        private readonly ServiceWorkerConfiguration _serviceWorkerConfiguration;
        string targetBaseUrl;
        private IThermoDataRequester thermoDataRequester;
        private CancellationToken _stoppingToken;
        private ICheckPointLogger _checkPointLogger;
        private int _errorCount = 0;
        private IChannelMessageSender _channelMessageSender;

        public ThermoDataLogic(ILogger<ThermoDataLogic> logger, IConfiguration configuration, ICheckPointLogger checkPointLogger, IChannelMessageSender channelMessageSender)
        {
            _logger = logger;
            _configuration = configuration;
            
            _restConfiguration = configuration.GetSection(ThermoRestApiConfigurationKey).Get<ThermoRestConfiguration>();
            _checkPointLogger = checkPointLogger;
            _serviceWorkerConfiguration = configuration.GetSection(ServiceWorkerConfigirationKey).Get<ServiceWorkerConfiguration>();
            _channelMessageSender = channelMessageSender;
        }

        public void Setup(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            _channelMessageSender.Setup(_stoppingToken);
        }

        public async Task ExecuteAsync()
        {
            try
            {
                await RunMainTaskAsync();
            }
            catch (Exception ex)
            {
                _errorCount++;
                _logger.LogError($"ThermoLogic component run into some issues : {ex.Message} - Error count {_errorCount} : {ex.StackTrace}");

                if (_errorCount > 5)
                {
                    _logger.LogError($"ThermoLogic max error count exceeded. {ex.Message}");
                    Environment.Exit(-1);
                }
            }
        }

        private async Task RunMainTaskAsync()
        {
            var tasks = new List<Task>();

            foreach (var targetDevice in _restConfiguration.TargetDevices)
            {
                _logger.LogInformation(MessageSpacer);
                _logger.LogInformation($"Scattering process for {targetDevice.HostName}, {DateTime.Now}");
                _logger.LogInformation(MessageSpacer);
            
                tasks.Add(RunTaskForTargetDevices(targetDevice));
            }

            await Task.WhenAll(tasks);
        }

        private async Task RunTaskForTargetDevices(TargetDevice targetDevice)
        {
            targetBaseUrl = targetDevice.HostName;
            thermoDataRequester = RequestFactory.CreateRestService(targetBaseUrl, _logger);
            var checkpointSourceFileName = targetDevice.CheckPointFileName;
            var checkPoint = await _checkPointLogger.ReadCheckPoint(checkpointSourceFileName);

            checkPoint.LastSequence = checkPoint.LastSequence == 0 ? 1 : checkPoint.LastSequence;

            var attendanceRequestInfo = new AttendanceRequest
            {
                StartId = checkPoint.LastSequence,
                ReqCount = targetDevice.AttendanceRequestCount == 0 ? 10 : targetDevice.AttendanceRequestCount,
                NeedImg = targetDevice.NeedImage
            };
            
            // parse request 
            var attendanceRequest = RequestFactory.CreatePostBodyRequest(
                targetDevice.AttendanceUrl, attendanceRequestInfo);

            _logger.LogInformation($"Executing ThermoDataLogic Main Component {DateTimeOffset.Now}");

            // Get data 
            var result = await thermoDataRequester.
                GetAttendanceRecordAsync<AttendanceResponse>(attendanceRequest);

            var attendanceRecResult = MessageConverter.DeSerializeCamelCase<AttendanceResponse>(result.Content);

            if (attendanceRecResult != null && attendanceRecResult.Command == 523)
            {
                _logger.LogError($"Invalida requrest made to the server: Status 523. {targetDevice.HostName}: {DateTime.Now}. Maybe your request count is zero. Please ensure it is atleast 1.");
            }
            else if (attendanceRecResult != null && attendanceRecResult.Data != null )
            {
                await this._channelMessageSender.SendMessagesToAzureServiceBus(attendanceRecResult);
                // Update configuration 
                checkPoint.LastSequence += attendanceRecResult.RecordCount;
                checkPoint.LastUpdate = DateTime.UtcNow;
                // Save checkpoint 
                await _checkPointLogger.WriteCheckPoint(checkpointSourceFileName, checkPoint);
            }
            else
            {
                // Send heartbeat messages //
                _logger.LogWarning($"No records retrieve from Rest Service. {targetDevice.HostName}: {DateTime.Now}");

                await this._channelMessageSender.SendHeartBeatMessagesToAzureServiceBus(targetDevice);
            }
        }
    }
}