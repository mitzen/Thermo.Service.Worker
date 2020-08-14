namespace Service.MessageBusServiceProvider.Queue
{
    public class NotificationServiceBusConfiguration
    {
        public string ServiceBusConnection { get; set; }
        public string QueueName { get; set; }
    }
}