using Microsoft.Azure.ServiceBus;

namespace MessageBusServiceProvider
{
    public class MessageBusServiceFactory
    {
        public IQueueClient CreateQueueClient(ServiceBusConfiguration option)
        {
            return new QueueClient(option.ServiceBusConnection, option.QueueName);
        }
    }
}