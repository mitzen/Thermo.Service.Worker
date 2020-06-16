using Microsoft.Azure.ServiceBus;

namespace MessageBusServiceProvider
{
    class MessageBusServiceFactory
    {
        public IQueueClient CreateQueueClient(QueueClientOption option)
        {
            return new QueueClient(option.ServiceBusConnectionName, option.QueueName);
        }
    }
}