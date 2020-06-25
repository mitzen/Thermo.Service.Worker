using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using AzCloudApp.MessageProcessor.Core.PersonelThemoDataHandler;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Configuration;

namespace AzCloudApp.MessageProcessor.Core.MessageSenderFunction
{
    public  class QueueMessageSenderTestFunction
    {
        private readonly IMesssageThermoProcessor _messsageThermoProcessor;
        private readonly ILogger<QueueMessageSenderTestFunction> _logger;
        private readonly NotificationConfiguration _optionsNotification;

        public QueueMessageSenderTestFunction(ILogger<QueueMessageSenderTestFunction> logger, IMesssageThermoProcessor messsageThermoProcessor, IOptions<NotificationConfiguration> options)
        {
            this._messsageThermoProcessor = messsageThermoProcessor;
            this._logger = logger;
            this._optionsNotification = options.Value;
        }

        [FunctionName("QueueMessageSenderTestFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
           const string ServiceBusConnectionString = 
                "Endpoint=sb://devsbbank.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZeoCedTJSaqVPAx8bHX998DVIYHtuG5g0OKlUkUFF9g=";
            const string QueueName = "devsbqbank";
            
            log.LogInformation($"Target queue name : {QueueName}");
            IQueueClient queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            var qms = new QueueMessageSender(queueClient);

            for (int i = 0; i < 10; i++)
            {
                var testMessage = new
                {
                    Name = "jeremy" + DateTime.Now,
                    Email = "kepung@gmail.com"
                };

                await qms.SendMessagesAsync(MessageConverter.Serialize(testMessage));
                log.LogInformation($"Sending data over {i} - {DateTime.Now}");
                await this._messsageThermoProcessor.ProcessMessage(MessageConverter.Serialize(testMessage));
            }
            
            return new OkObjectResult($"Sent operation completed! :{DateTime.Now}.");
        }
    }
}
