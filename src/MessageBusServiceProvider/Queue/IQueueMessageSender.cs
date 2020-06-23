using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.Queue
{
    public interface IQueueMessageSender
    {
        Task SendMessagesAsync(string messageSource);
    }
}