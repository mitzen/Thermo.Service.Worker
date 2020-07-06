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
using Service.MessageBusServiceProvider.AzBlob;
using Service.MessageBusServiceProvider.Imaging;
using System.IO;
using Service.MessageBusServiceProvider.IOUtil;

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
        private const string DefaultImageJpg = ".jpg";
        private readonly IBlobClientProvider _blobClientProvider;
        private readonly BlobConfiguration _blobConfiguration;

        public ChannelMessageSender(ILogger<ChannelMessageSender> logger,
            IConfiguration configuration, IBlobClientProvider blobClientProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _blobClientProvider = blobClientProvider;

            _serviceBusConfiguration = configuration.GetSection(ServiceBusConfigurationKey).Get<ServiceBusConfiguration>();

            _blobConfiguration = BlobConfigurationUtil.GetBlobConfigiration(configuration);

        }

        public void Setup(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_serviceBusConfiguration, _logger);

            FileUtil.CreateLocalDirectoriesInPath(_blobConfiguration.ImageStorePath);
        }

        public async Task SendMessagesToAzureServiceBus(AttendanceResponse attendanceRecResult)
        {
            var currentBatchId = Guid.NewGuid().ToString();

            foreach (var attendanceItem in attendanceRecResult.Data)
            {
                attendanceItem.MessageType = CoreMessageType.AttendanceMessage;
                attendanceItem.BatchId = currentBatchId;
                attendanceItem.Birth ??= DateTime.MinValue;
                attendanceItem.TimeStamp ??= DateTime.MinValue;

                if (double.IsNaN(attendanceItem.BodyTemperature))
                {
                    attendanceItem.BodyTemperature = 0d;
                    this._logger.LogInformation($"Temperature is NaN setting it to : {attendanceItem.BodyTemperature}");
                }

                if (!string.IsNullOrWhiteSpace(attendanceItem.Img))
                {
                    // All these can be done async 

                    var targetImagePath = $"{_blobConfiguration.ImageStorePath}{Path.DirectorySeparatorChar}{attendanceItem.Id}{DefaultImageJpg}";

                    // Save Images 
                    ImageConverter.SaveByteArrayAsImage(targetImagePath, ImageConverter.ExractBase64(attendanceItem.Img));

                    // Push image to Azure // 
                    await this._blobClientProvider.PushImageToStoreAsync(_blobConfiguration.ContainerName, targetImagePath);

                    attendanceItem.Img = $"{_blobConfiguration.StorageEndpoint}/{attendanceItem.Id}{DefaultImageJpg}";

                    this._logger.LogInformation($"Saving images: {attendanceItem.Id} to {targetImagePath} to cloud path : {_blobConfiguration.StorageEndpoint}/{_blobConfiguration.ContainerName} : {DateTime.Now}");

                }

                var messgeInstance = MessageConverter.Serialize(attendanceItem);
                await _messageSender.SendMessagesAsync(messgeInstance);
            }

            this._logger.LogInformation($"{currentBatchId} : Batch sent {DateTime.Now}");
            //return Task.CompletedTask;
        }

        public Task SendHeartBeatMessagesToAzureServiceBus(TargetDevice targetDevice)
        {
            var heartbeatMessage = new HeartbeatMessage
            {
                MessageType = CoreMessageType.HeartBeatMessage,
                Status = OnlineMessage,
                DeviceId = targetDevice.HostName,
                Timestamp = DateTime.UtcNow
            };

            var messgeInstance = MessageConverter.Serialize(heartbeatMessage);
            _messageSender.SendMessagesAsync(messgeInstance);
            this._logger.LogInformation($"Sending HeartBeat message {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
