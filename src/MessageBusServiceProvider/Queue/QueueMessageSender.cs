using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Service.MessageBusServiceProvider.Queue
{
    public class QueueMessageSender : IQueueMessageSender
    {
        private readonly IQueueClient _queueClient;
        private readonly ILogger _logger;

        public QueueMessageSender(IQueueClient queueClient, ILogger logger)
        {
            this._queueClient = queueClient;
            _logger = logger;
        }

        public async Task SendMessagesAsync(string messageSource)
        {
            this._logger.LogInformation($"QueueMessageSender : Sending message.");

            var message = new Message(Encoding.UTF8.GetBytes(messageSource));
            await this._queueClient.SendAsync(message);
        }
    }
}