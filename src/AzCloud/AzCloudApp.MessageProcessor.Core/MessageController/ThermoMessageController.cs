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

        //var messsageType = GetMessageType(sourceData);
        //_ = messsageType.MessageType switch
        //        {
        //            (0) => _dataStoreProcesor.SavePersonAsync(sourceData),
        //            (1) => _dataStoreProcesor.SavePersonImgAsync(sourceData),
        //            (2) => _dataStoreProcesor.SaveDevicesAsync(sourceData),
        //            (3) => _dataStoreProcesor.SaveAttendRecordAsync(sourceData)
        //};

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

                logger.LogInformation($" ********** Persist to database ********* : { attendanceRecFromQueue.Id } ");

                //switch (messsageType.MessageType)
                //{
                //    case 0:
                //        this._logger.LogInformation(PersonProcessingMessage);
                //        _dataStoreProcesor.SavePersonAsync(sourceData);
                //        break;
                //    case 1:
                //        this._logger.LogInformation(ImageProcessingMessage);
                //        _dataStoreProcesor.SavePersonImgAsync(sourceData);
                //        break;
                //    case 2:
                //        this._logger.LogInformation(DeviceProcessingMessage);
                //        _dataStoreProcesor.SaveDevicesAsync(sourceData);
                //        break;
                //    case 3:
                //        this._logger.LogInformation(AttendanceMessageProcessingMessage);
                //        _dataStoreProcesor.SaveAttendRecordAsync(sourceData);
                //        _notificationProcesor.ProcessAsync(sourceData);
                //        break;
                //    default:
                //        break;
                //}
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
