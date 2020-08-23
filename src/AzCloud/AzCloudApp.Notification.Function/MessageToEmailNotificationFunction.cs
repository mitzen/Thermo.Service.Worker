using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Configuration;
using System.Threading.Tasks;

namespace AzCloudApp.Notification.Function
{
    public class MessageToEmailNotificationFunction
    {
        private readonly ILogger<MessageToEmailNotificationFunction> _logger;
        private readonly INotificationProcessor _notificationProcessor;
        private NotificationConfiguration _options;
        public MessageToEmailNotificationFunction(ILogger<MessageToEmailNotificationFunction> logger, INotificationProcessor notificationMessageProcessor, IOptions<NotificationConfiguration> options)
        {
            this._logger = logger;
            this._notificationProcessor = notificationMessageProcessor;
            this._options = options.Value;
        }

        [FunctionName("MessageToEmailNotificationFunction")]
        public async Task Run([ServiceBusTrigger("%TargetQueueName%", Connection = "sbqconnection")] string messageSource, ILogger logger)
        {
           await this._notificationProcessor.ProcessAsync(messageSource, logger);
        }
    }
}
