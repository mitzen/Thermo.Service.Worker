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
using System.IO;

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

        public ThermoDataLogic(ILogger logger, IConfiguration configuration, CancellationToken token, ICheckPointLogger checkPointLogger)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceBusConfiguration = configuration.GetSection(ServiceBusConfigurationKey).Get<ServiceBusConfiguration>();
            _restConfiguration = configuration.GetSection(ThermoRestApiConfigurationKey).Get<ThermoRestConfiguration>();
            _stoppingToken = token;
            _checkPointLogger = checkPointLogger;
            _serviceWorkerConfiguration = configuration.GetSection(ServiceWorkerConfigirationKey).Get<ServiceWorkerConfiguration>();
        }

        public void Setup()
        {
            targetBaseUrl = _restConfiguration.HostName;
            thermoDataRequester = RequestFactory.CreateRestService(targetBaseUrl, _stoppingToken, _logger);
            _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_serviceBusConfiguration, _logger);
        }

        public async Task ExecuteAsync()
        {
            // read checpoint info

            var checkpointSourceFileName = _serviceWorkerConfiguration.CheckPointFilePath  + Path.DirectorySeparatorChar + @"chkpoint1.json";

            var checkPoint = await _checkPointLogger.ReadCheckPoint(checkpointSourceFileName);

            var attendanceRequestInfo = new AttendanceRequest
            {
                StartId = checkPoint.LastSequence,
                ReqCount = 20,
                NeedImg = false
            };

            // parse request 
            var attendanceRequest = RequestFactory.CreatePostBodyRequest(
                _restConfiguration.AttendanceUrl, attendanceRequestInfo);

            _logger.LogInformation($"Executing ThermoDataLogic {DateTimeOffset.Now}");

            // Get data 
            var result = await thermoDataRequester.
                GetAttendanceRecordAsync<AttendanceResponse>(attendanceRequest);

            _logger.LogInformation(result.Content);

            var attendanceRecResult = MessageConverter.DeSerializeCamelCase<AttendanceResponse>(result.Content);

            if (attendanceRecResult != null)
            {
                await SendMessagesToAzureServiceBus(attendanceRecResult);
            }

            checkPoint.LastSequence += attendanceRequestInfo.ReqCount;

            await _checkPointLogger.WriteCheckPoint(checkpointSourceFileName, checkPoint);
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
                this._logger.LogInformation($"Sending message to service bus : {attendanceItem.Id} ");
            }

            this._logger.LogInformation($"{currentBatchId} : Batch sent {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}