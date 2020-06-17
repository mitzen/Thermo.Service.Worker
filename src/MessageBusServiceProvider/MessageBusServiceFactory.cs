using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace MessageBusServiceProvider
{
    public class MessageBusServiceFactory
    {
        public static IQueueClient CreateQueueClient(ServiceBusConfiguration option)
        {
            return new QueueClient(option.ServiceBusConnection, option.QueueName);
        }

        public static IQueueMessageSender CreateSender(ServiceBusConfiguration option, ILogger logger)
        {
            var client = CreateQueueClient(option);
            return new QueueMessageSender(client, logger);
        }
    }
}