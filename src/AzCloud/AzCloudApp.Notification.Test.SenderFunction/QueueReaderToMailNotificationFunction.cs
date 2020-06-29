using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Service.ThermoDataModel.Configuration;

namespace AzCloudApp.Notification.Test.SenderFunction
{
    public class QueueReaderToMailNotificationFunction
    {
        private readonly INotificationProcessor _notificationProcessor;
        private readonly ILogger<QueueReaderToMailNotificationFunction> _logger;
        private NotificationConfiguration _notificationConfiguration;

        public QueueReaderToMailNotificationFunction(ILogger<QueueReaderToMailNotificationFunction> logger,
            INotificationProcessor notificationProcessor, IOptions<NotificationConfiguration> options)
        {
            _logger = logger;
            _notificationProcessor = notificationProcessor;
            _notificationConfiguration = options.Value;
        }

        [FunctionName("QueueReaderToMailNotificationFunctionHttpTest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger logger)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            /// THIS IS INTENTED TO BE A TEST FUNCTION TRIGGERED USING HTTP ///
            ////////////////////////////////////////////////////////////////////////////////////////
            
            var requestBody = "";
            await this._notificationProcessor.ProcessAsync("", logger);
            return new OkObjectResult($"Reading messages from the queue.{DateTime.Now} {requestBody}");
        }
    }
}
