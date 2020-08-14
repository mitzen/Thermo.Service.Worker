using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.MessageBusServiceProvider.Converters;
using Service.MessageBusServiceProvider.Queue;
using Service.ThermoDataModel.Models;
using System;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.AttendanceDataRuleFilter
{
    public class TemperatureDataFilter : IDataFilter
    {
        private const string AttendanceRecordNullMessage = "AttendanceRecord is null";
        private IQueueMessageSender _messageSender;
        private ServiceBusConfiguration _notificationServiceBusConfiguration;
        private TemperatureFilterConfiguration _temperatureFilterConfiguration;

        public TemperatureDataFilter(IOptions<ServiceBusConfiguration> notificationServiceBusOption,
            IOptions<TemperatureFilterConfiguration> temperatureOption)
        {
            _notificationServiceBusConfiguration = notificationServiceBusOption.Value;
            _temperatureFilterConfiguration = temperatureOption.Value;
        }

        public async Task ExecuteDataFiltering(AttendanceRecord attendanceRecord, ILogger logger)
        {
            if (attendanceRecord == null)
                throw new ArgumentException(AttendanceRecordNullMessage);

            logger.LogInformation($"TemperatureDataFilter sbconnection : {_notificationServiceBusConfiguration.ServiceBusConnection}, queue : {_notificationServiceBusConfiguration.QueueName} and temperature max : {_temperatureFilterConfiguration.Max} ");

            if (attendanceRecord.BodyTemperature > _temperatureFilterConfiguration.Max)
            {
                _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_notificationServiceBusConfiguration, logger);
                
                var messgeInstance = MessageConverter.Serialize(attendanceRecord);
                await _messageSender.SendMessagesAsync(messgeInstance);
                // format template 
            }
        }
    }
}
