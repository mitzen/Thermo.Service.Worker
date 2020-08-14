using AzCloudApp.MessageProcessor.Core.AttendanceDataRuleFilter;
using AzCloudApp.MessageProcessor.Core.DataProcessor;
using AzCloudApp.MessageProcessor.Core.Utils;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel;
using Service.ThermoDataModel.Heartbeat;
using Service.ThermoDataModel.Models;
using System;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.MessageController
{
    public class ThermoMessageController : IMessageController
    {
        private ILogger _logger;
        private readonly IDataStoreProcesor _dataStoreProcesor;
        private readonly INotificationProcessor _notificationProcesor;
        private readonly IDataFilter _dataFilter;

        public ThermoMessageController(IDataStoreProcesor dataStoreProcesor, INotificationProcessor notificationProcesor, IDataFilter dataFilter)
        {
            _dataStoreProcesor = dataStoreProcesor;
            _notificationProcesor = notificationProcesor;
            _dataFilter = dataFilter;
        }

        public Task ProcessDataAsync(string sourceData, ILogger logger)
        {
            this._logger = logger;

            logger.LogInformation($"MessageController::ProcessDataAsync [Deserializing messages] : {DateTime.Now}");

            try
            {
                var attendanceRecFromQueue = MessageConverter.GetMessageType<AttendanceRecord>(sourceData);

                if (attendanceRecFromQueue.MessageType == "TEST")
                {
                    logger.LogInformation($"Test data detected : { attendanceRecFromQueue.Id }");
                    return Task.CompletedTask;
                }
                else if (attendanceRecFromQueue.MessageType == CoreMessageType.AttendanceMessage)
                {
                    _dataFilter.ExecuteDataFiltering(attendanceRecFromQueue, logger);

                    var result = this._dataStoreProcesor.SaveAttendanceRecordAsync(attendanceRecFromQueue);
                    logger.LogInformation($" ********** Saved attendance record to database ********* : { attendanceRecFromQueue.Id } and record count  {result.Result}");
                }
                else if (attendanceRecFromQueue.MessageType == CoreMessageType.HeartBeatMessage)
                {
                    var heartbeat = MessageConverter.GetMessageType<HeartbeatMessage>(sourceData);

                    var result = this._dataStoreProcesor.SaveHeartBeatRecordAsync(heartbeat);
                    logger.LogInformation($" ********** Saved HeartBeat record to database ********* : { attendanceRecFromQueue.Id } and record count  {result.Result}");
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
