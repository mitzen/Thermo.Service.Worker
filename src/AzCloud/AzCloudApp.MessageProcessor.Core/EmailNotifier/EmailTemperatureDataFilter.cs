using AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.MessageBusServiceProvider.Converters;
using Service.MessageBusServiceProvider.Queue;
using Service.ThermoDataModel.Models;
using System;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.EmailNotifier
{
    public class EmailTemperatureDataFilter : IDataFilter
    {
        private const string AttendanceRecordNullMessage = "AttendanceRecord is null";
        private IQueueMessageSender _messageSender;
        private ServiceBusConfiguration _notificationServiceBusConfiguration;
        private TemperatureFilterConfiguration _temperatureFilterConfiguration;
        private readonly IMailContentParser _mailContentParser;
        public EmailTemperatureDataFilter(IOptions<ServiceBusConfiguration> notificationServiceBusOption,
            IOptions<TemperatureFilterConfiguration> temperatureOption,
            IMailContentParser mailContentParser)
        {
            _notificationServiceBusConfiguration = notificationServiceBusOption.Value;
            _temperatureFilterConfiguration = temperatureOption.Value;
            _mailContentParser = mailContentParser;
        }

        public async Task ExecuteDataFiltering(AttendanceRecord attendanceRecord, ILogger logger)
        {
            try
            {
                if (attendanceRecord == null)
                    throw new ArgumentException(AttendanceRecordNullMessage);

                logger.LogInformation($"Running TemperatureDataFilter sbconnection : {_notificationServiceBusConfiguration.ServiceBusConnection}, queue : {_notificationServiceBusConfiguration.QueueName} and temperature max : {_temperatureFilterConfiguration.Max} ");

                if (attendanceRecord.BodyTemperature > _temperatureFilterConfiguration.Max)
                {
                    _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_notificationServiceBusConfiguration, logger);

                    var mailParam = new EmailTemperatureHitParameter(attendanceRecord.DeviceId, _temperatureFilterConfiguration.EmailTemplateMaxTemperatureHit);
                    
                    mailParam.Location = attendanceRecord.Address;
                    mailParam.TemperatureRegistered = attendanceRecord.BodyTemperature;
                    mailParam.Image = attendanceRecord.Img;
                    mailParam.Timestamp = attendanceRecord.TimeStamp;

                    var mailData = _mailContentParser.CreateTemperatureEMailAlertMessage(mailParam, logger);

                    if (mailData != null)
                    {
                        var messgeInstance = MessageConverter.Serialize(mailData);
                        await _messageSender.SendMessagesAsync(messgeInstance);

                        logger.LogInformation("MailData message sent to queue.");
                    }
                    else
                    {
                        logger.LogInformation($"No notifcation sent to queue. {DateTime.Now}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Notfication filter failed. {ex.Message}. {ex.StackTrace}");
            }
        }
    }
}
