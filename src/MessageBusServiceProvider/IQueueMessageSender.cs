using System.Threading.Tasks;

namespace MessageBusServiceProvider
{
    public interface IQueueMessageSender
    {
        Task SendMessagesAsync(string messageSource);
    }
}