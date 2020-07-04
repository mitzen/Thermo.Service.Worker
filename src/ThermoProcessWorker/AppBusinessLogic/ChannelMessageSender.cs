using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using Service.MessageBusServiceProvider.Converters;
using Service.ThermoDataModel.Heartbeat;
using Service.ThermoDataModel;
using Service.MessageBusServiceProvider.Queue;
using Microsoft.Extensions.Configuration;

namespace Service.ThermoProcessWorker.AppBusinessLogic
{
    public class ChannelMessageSender : IChannelMessageSender
    {
        private IQueueMessageSender _messageSender;
        private const string OnlineMessage = "ONLINE";
        private ILogger<ChannelMessageSender> _logger;
        private CancellationToken _stoppingToken;
        private readonly ServiceBusConfiguration _serviceBusConfiguration;
        private readonly IConfiguration _configuration;
        private const string ServiceBusConfigurationKey = "ServiceBusConfiguration";

        public ChannelMessageSender(ILogger<ChannelMessageSender> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _serviceBusConfiguration = configuration.GetSection(ServiceBusConfigurationKey).Get<ServiceBusConfiguration>();
        }

        public void Setup(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_serviceBusConfiguration, _logger);
        }

        public Task SendMessagesToAzureServiceBus(AttendanceResponse attendanceRecResult)
        {
            var currentBatchId = Guid.NewGuid().ToString();

            foreach (var attendanceItem in attendanceRecResult.Data)
            {
                attendanceItem.MessageType = CoreMessageType.AttendanceMessage;

                attendanceItem.Id = Guid.NewGuid().ToString();
                attendanceItem.BatchId = currentBatchId;

                attendanceItem.Birth ??= DateTime.MinValue;
                attendanceItem.TimeStamp ??= DateTime.MinValue;

                var messgeInstance = MessageConverter.Serialize(attendanceItem);
                _messageSender.SendMessagesAsync(messgeInstance);
            }

            this._logger.LogInformation($"{currentBatchId} : Batch sent {DateTime.Now}");
            return Task.CompletedTask;
        }

        public Task SendHeartBeatMessagesToAzureServiceBus(TargetDevice targetDevice)
        {
            var heartbeatMessage = new HeartbeatMessage
            {
                MessageType = CoreMessageType.HeartBeatMessage,
                Status = OnlineMessage,
                DeviceId = targetDevice.HostName,
                Timestamp = DateTime.Now
            };

            var messgeInstance = MessageConverter.Serialize(heartbeatMessage);
            _messageSender.SendMessagesAsync(messgeInstance);
            this._logger.LogInformation($"Sending HeartBeat message {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
