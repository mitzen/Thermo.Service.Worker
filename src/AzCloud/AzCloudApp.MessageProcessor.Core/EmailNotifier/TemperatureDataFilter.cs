﻿using AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Service.MessageBusServiceProvider.Converters;
using Service.MessageBusServiceProvider.Queue;
using Service.ThermoDataModel.Models;
using System;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.EmailNotifier
{
    public class TemperatureDataFilter : IDataFilter
    {
        private const string AttendanceRecordNullMessage = "AttendanceRecord is null";
        private IQueueMessageSender _messageSender;
        private ServiceBusConfiguration _notificationServiceBusConfiguration;
        private TemperatureFilterConfiguration _temperatureFilterConfiguration;
        private readonly IMailContentParser _mailContentParser;
        public TemperatureDataFilter(IOptions<ServiceBusConfiguration> notificationServiceBusOption,
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

                    var mailParam = new EmailInfoParameter(attendanceRecord.DeviceId, _temperatureFilterConfiguration.EmailTemplateMaxTemperatureHit);
                    
                    mailParam.Location = attendanceRecord.Address;
                    mailParam.TemperatureRegistered = attendanceRecord.BodyTemperature;
                    mailParam.Image = attendanceRecord.Img;
                    mailParam.Timestamp = attendanceRecord.TimeStamp;

                    var mailData = _mailContentParser.CreateMailMessage(mailParam);

                    var messgeInstance = MessageConverter.Serialize(mailData);
                    await _messageSender.SendMessagesAsync(messgeInstance);

                    logger.LogInformation("MailData message sent to queue.");
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation($"Notfication filter failed. {ex.Message}. {ex.StackTrace}");
            }
        }
    }
}
