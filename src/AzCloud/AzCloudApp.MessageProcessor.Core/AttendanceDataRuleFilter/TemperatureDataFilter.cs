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
        private IQueueMessageSender _messageSender;
        private NotificationServiceBusConfiguration _notificationServiceBusConfiguration;
        private ServiceBusConfiguration _serviceBusConfiguration;

        public TemperatureDataFilter(IOptions<NotificationServiceBusConfiguration> options)
        {
            _notificationServiceBusConfiguration = options.Value;
        }

        public async Task ExecuteDataFiltering(AttendanceRecord attendanceRecord, ILogger logger)
        {
            if (attendanceRecord == null)
                throw new ArgumentException("attendanceRecord is null");

            logger.LogInformation($"TemperatureDataFilter sbconnection : {_notificationServiceBusConfiguration.ServiceBusConnection}, queue : {_notificationServiceBusConfiguration.QueueName} ");

            if (attendanceRecord.BodyTemperature > 37d)
            {
                _serviceBusConfiguration = new ServiceBusConfiguration(_notificationServiceBusConfiguration.ServiceBusConnection, _notificationServiceBusConfiguration.QueueName);

                _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_serviceBusConfiguration, logger);
                
                // get from template and then write to queue 
                var messgeInstance = MessageConverter.Serialize(attendanceRecord);
                await _messageSender.SendMessagesAsync(messgeInstance);
                // format template 
            }
        }
    }
}
