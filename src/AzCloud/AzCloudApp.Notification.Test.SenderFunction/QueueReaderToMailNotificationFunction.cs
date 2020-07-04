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

namespace AzCloudApp.Notification.Test.SenderFunction
{
    public class QueueReaderToMailNotificationFunction
    {
        private readonly INotificationProcessor _notificationProcessor;
        private readonly ILogger<QueueReaderToMailNotificationFunction> _logger;
        private NotificationConfiguration _notificationConfiguration;
        private ThermoDataContext _thermoDataContext;
        private IDataStoreProcesor _dataProcessor;
        public QueueReaderToMailNotificationFunction(ILogger<QueueReaderToMailNotificationFunction> logger, IDataStoreProcesor dataProcessor, IOptions<NotificationConfiguration> options, ThermoDataContext dbContext)
        {   
            _logger = logger;
            //_notificationProcessor = notificationProcessor;
            _dataProcessor = dataProcessor;
            _thermoDataContext = dbContext;
        }

        [FunctionName("QueueReaderToMailNotificationFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            //await TestAttendanceRecord();

            var h = new HeartbeatMessage()
            {
                Status = "ONLINE",
                Timestamp = DateTime.UtcNow,
                DeviceId = "100",
            };

            
            var x = await this._dataProcessor.SaveHeartBeatRecordAsync(h);
          
            var requestBody = "";
            return new OkObjectResult($"Reading messages from the queue.{DateTime.Now} {requestBody}");
        }

        private async Task TestAttendanceRecord()
        {
            var atendanceRcord = new AttendanceRecord
            {
                Address = "address",
                Age = 10,
                BatchId = Guid.NewGuid().ToString(),
                Birth = DateTime.Now,
                BodyTemperature = 30.1f,
                CertificateNumber = "100",
                CertificateType = 0,
                Country = "NZ",
                DeviceId = "DeviceId",
                Gender = "M",
                Name = "Name",
                PersonId = "PersonId",
                TimeStamp = DateTime.UtcNow
            };


            var result = await this._dataProcessor.SaveAttendanceRecordAsync(atendanceRcord);
        }
    }
}
