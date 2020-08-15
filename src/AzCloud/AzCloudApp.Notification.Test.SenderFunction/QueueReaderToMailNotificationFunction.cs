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
using Microsoft.EntityFrameworkCore;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System.Linq;
using Service.ThermoDataModel.Models;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Microsoft.Azure.Amqp.Framing;
using Service.ThermoDataModel.Heartbeat;
using AzCloudApp.MessageProcessor.Core.PersonelThemoDataHandler;

namespace AzCloudApp.Notification.Test.SenderFunction
{
    public class QueueReaderToMailNotificationFunction
    {
    
        private readonly IMesssageThermoProcessor _messsageThermoProcessor;
        private readonly ILogger<QueueReaderToMailNotificationFunction> _logger;
        
        public QueueReaderToMailNotificationFunction(ILogger<QueueReaderToMailNotificationFunction> logger,
            IMesssageThermoProcessor messsageThermoProcessor)
        {
            this._messsageThermoProcessor = messsageThermoProcessor;
            _logger = logger;
        }

        [FunctionName("QueueReaderToMailNotificationFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {

            try
            {
                await this._messsageThermoProcessor.ProcessMessage("", _logger);
            }
            catch (Exception ex)
            {
                throw;
            }


            return new OkObjectResult($"Reading messages from the queue.{DateTime.Now}");
        }

      
    }
}
