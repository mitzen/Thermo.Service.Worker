using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider
{
    public interface IQueueMessageSender
    {
        Task SendMessagesAsync(string messageSource);
    }
}