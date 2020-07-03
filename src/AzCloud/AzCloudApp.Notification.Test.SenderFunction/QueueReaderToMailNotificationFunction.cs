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
using Service.ThermoDataModel.Models;
using System.Linq;

namespace AzCloudApp.Notification.Test.SenderFunction
{
    public class QueueReaderToMailNotificationFunction
    {
        private readonly INotificationProcessor _notificationProcessor;
        private readonly ILogger<QueueReaderToMailNotificationFunction> _logger;
        private NotificationConfiguration _notificationConfiguration;
        private ThermoDataContext _thermoDataContext;
        public QueueReaderToMailNotificationFunction(ILogger<QueueReaderToMailNotificationFunction> logger, INotificationProcessor notificationProcessor, IOptions<NotificationConfiguration> options, ThermoDataContext dbContext)
        {   
            _logger = logger;
            _notificationProcessor = notificationProcessor;
            _notificationConfiguration = options.Value;
            _thermoDataContext = dbContext;
        }

        [FunctionName("QueueReaderToMailNotificationFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            //var body = new StreamReader(req.Body);
            //body.BaseStream.Seek(0, SeekOrigin.Begin);
            //var requestBody = body.ReadToEnd();
            // await this._notificationProcesso

            var entity = _thermoDataContext.AttendanceRecord.Where(x => x.Name == "Tong").FirstOrDefault();
            
            if (entity != null)
            {
                var name =  entity.Name;
            }

            var requestBody = "";
            //await this._notificationProcessor.ProcessAsync("");

            return new OkObjectResult($"Reading messages from the queue.{DateTime.Now} {requestBody}");
        }
    }
}
