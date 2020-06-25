using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.MessageSenderFunction
{
    class QueueMessageSender
    {
        private readonly IQueueClient _queueClient;

        public QueueMessageSender(IQueueClient queueClient)
        {
            this._queueClient = queueClient;
        }

        public async Task SendMessagesAsync(string messageSource)
        {
            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(messageSource));
                await this._queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {

            }
        }
    }
}
