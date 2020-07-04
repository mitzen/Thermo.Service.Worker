using AzCloudApp.MessageProcessor.Core.DataProcessor;
using AzCloudApp.MessageProcessor.Core.Utils;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;
using System;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.MessageController
{
    public class ThermoMessageController : IMessageController
    {
        private const string AttendanceMessageProcessingMessage = "[ATTENDANCE] processing record";
        private const string ImageProcessingMessage = "[IMAGE] processing record";
        private const string DeviceProcessingMessage = "[DEVICE] processing record ";
        private const string PersonProcessingMessage = "[PERSON DATA] processing record.";

        private readonly ILogger<ThermoMessageController> _logger;
        private readonly IDataStoreProcesor _dataStoreProcesor;
        private readonly INotificationProcessor _notificationProcesor;

        public ThermoMessageController(ILogger<ThermoMessageController> logger,
            IDataStoreProcesor dataStoreProcesor, INotificationProcessor notificationProcesor)
        {
            _logger = logger;
            _dataStoreProcesor = dataStoreProcesor;
            _notificationProcesor = notificationProcesor;
        }

        public Task ProcessDataAsync(string sourceData, ILogger logger)
        {
            logger.LogInformation($"MessageController::ProcessDataAsync [Deserializing messages] : {DateTime.Now}");

            try
            {
                var attendanceRecFromQueue = MessageConverter.GetMessageType<AttendanceRecord>(sourceData);

                if (attendanceRecFromQueue.Subject == "TEST")
                {
                    logger.LogInformation($"Test data detected : { attendanceRecFromQueue.Id }");
                    return Task.CompletedTask;
                }
                else
                {   
                    var result = this._dataStoreProcesor.SaveAttendanceRecordAsync(attendanceRecFromQueue);
                    logger.LogInformation($" ********** Saved attendance record to database ********* : { attendanceRecFromQueue.Id } and record count  {result.Result}");

                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in MessageConroller : {ex.Message}");
                logger.LogError($"StackTrace: {ex.StackTrace}");
                throw;
            }
            return Task.CompletedTask;
        }
    }
}
