namespace Service.MessageBusServiceProvider.Queue
{
    public class ServiceBusConfiguration
    {
        public ServiceBusConfiguration()
        {

        }

        public ServiceBusConfiguration(string sbConnection, string targetQueueName)
        {
            ServiceBusConnection = sbConnection;
            QueueName = targetQueueName;
        }

        public string ServiceBusConnection { get; set; }
        public string QueueName { get; set; }
    }
}