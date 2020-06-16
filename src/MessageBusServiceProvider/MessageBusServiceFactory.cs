using Microsoft.Azure.ServiceBus;

namespace MessageBusServiceProvider
{
    public class MessageBusServiceFactory
    {
        public IQueueClient CreateQueueClient(QueueClientOption option)
        {
            return new QueueClient(option.ServiceBusConnectionName, option.QueueName);
        }
    }
}