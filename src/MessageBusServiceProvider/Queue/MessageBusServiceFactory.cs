using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Service.MessageBusServiceProvider.Queue
{
    public class MessageBusServiceFactory
    {
        public static IQueueClient CreateQueueClient(ServiceBusConfiguration option)
        {
            return new QueueClient(option.ServiceBusConnection, option.QueueName);
        }

        public static IQueueMessageSender CreateServiceBusMessageSender(ServiceBusConfiguration serviceBusOption, ILogger logger)
        {
            logger.LogInformation($"CreateServiceBusMessageSender. {serviceBusOption.QueueName}, {serviceBusOption.ServiceBusConnection}");
            var client = CreateQueueClient(serviceBusOption);
            return new QueueMessageSender(client, logger);
        }
    }
}