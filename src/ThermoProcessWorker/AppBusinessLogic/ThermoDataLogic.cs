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
using Service.MessageBusServiceProvider.Queue;
using Service.MessageBusServiceProvider.Converters;
using Service.MessageBusServiceProvider.CheckPointing;
using System.Collections.Generic;

namespace Service.ThermoProcessWorker.AppBusinessLogic
{
    public class ThermoDataLogic : IThermoDataLogic
    {
        private const string ServiceBusConfigurationKey = "ServiceBusConfiguration";
        private const string ThermoRestApiConfigurationKey = "ThermoRestConfiguration";
        private const string ServiceWorkerConfigirationKey = "ServiceWorkerConfiguration";
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ThermoRestConfiguration _restConfiguration;
        private readonly ServiceBusConfiguration _serviceBusConfiguration;
        private readonly ServiceWorkerConfiguration _serviceWorkerConfiguration;
        string targetBaseUrl;
        private IThermoDataRequester thermoDataRequester;
        private CancellationToken _stoppingToken;
        private IQueueMessageSender _messageSender;
        private ICheckPointLogger _checkPointLogger;
        private int _errorCount = 0;

        public ThermoDataLogic(ILogger<ThermoDataLogic> logger, IConfiguration configuration, ICheckPointLogger checkPointLogger)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceBusConfiguration = configuration.GetSection(ServiceBusConfigurationKey).Get<ServiceBusConfiguration>();
            _restConfiguration = configuration.GetSection(ThermoRestApiConfigurationKey).Get<ThermoRestConfiguration>();
            _checkPointLogger = checkPointLogger;
            _serviceWorkerConfiguration = configuration.GetSection(ServiceWorkerConfigirationKey).Get<ServiceWorkerConfiguration>();
        }

        public void Setup(CancellationToken token)
        {
            _stoppingToken = token;
            _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_serviceBusConfiguration, _logger);
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
                _logger.LogError($"ThermoLogic component run into some issues : {ex.Message} - Error count {_errorCount}");

                if (_errorCount > 5)
                {
                    Environment.Exit(-1);
                }
            }
        }

        private async Task RunMainTaskAsync()
        {
            var tasks = new List<Task>();

            foreach (var targetDevice in _restConfiguration.TargetDevices)
            {
                _logger.LogInformation($"##############################################################");
                _logger.LogInformation($"Scattering process for {targetDevice.HostName}, {DateTime.Now}");
                _logger.LogInformation($"##############################################################");
                tasks.Add(RunTaskForTargetDevices(targetDevice));
            }

            await Task.WhenAll(tasks);
        }

        private async Task RunTaskForTargetDevices(TargetDevice targetDevice)
        {
            targetBaseUrl = targetDevice.HostName;
            thermoDataRequester = RequestFactory.CreateRestService(targetBaseUrl, _stoppingToken, _logger);
            var checkpointSourceFileName = targetDevice.CheckPointFileName;
            var checkPoint = await _checkPointLogger.ReadCheckPoint(checkpointSourceFileName);
            var attendanceRequestInfo = new AttendanceRequest
            {
                StartId = checkPoint.LastSequence,
                ReqCount = targetDevice.AttendanceRequestCount == 0 ? 10 : targetDevice.AttendanceRequestCount,
                NeedImg = false
            };
            
            // parse request 
            var attendanceRequest = RequestFactory.CreatePostBodyRequest(
                targetDevice.AttendanceUrl, attendanceRequestInfo);

            _logger.LogInformation($"Executing ThermoDataLogic Main Component {DateTimeOffset.Now}");

            // Get data 
            var result = await thermoDataRequester.
                GetAttendanceRecordAsync<AttendanceResponse>(attendanceRequest);

            var attendanceRecResult = MessageConverter.DeSerializeCamelCase<AttendanceResponse>(result.Content);

            if (attendanceRecResult != null)
            {
                await SendMessagesToAzureServiceBus(attendanceRecResult);
                // Update configuration 
                checkPoint.LastSequence += attendanceRequestInfo.ReqCount;
                checkPoint.LastUpdate = DateTime.Now;
                // Save checkpoint 
                await _checkPointLogger.WriteCheckPoint(checkpointSourceFileName, checkPoint);
            }
            else
            {
                _logger.LogWarning($"No records retrieve from Rest Service. {targetDevice.HostName}: {DateTime.Now}");
            }
        }

        private Task SendMessagesToAzureServiceBus(AttendanceResponse attendanceRecResult)
        {
            var currentBatchId = Guid.NewGuid().ToString();

            foreach (var attendanceItem in attendanceRecResult.Data)
            {
                attendanceItem.Id = Guid.NewGuid().ToString();
                attendanceItem.BatchId = currentBatchId;
                var messgeInstance = MessageConverter.Serialize(attendanceItem);
                _messageSender.SendMessagesAsync(messgeInstance);
            }

            this._logger.LogInformation($"{currentBatchId} : Batch sent {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}