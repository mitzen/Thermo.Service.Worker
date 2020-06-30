using AzCloudApp.MessageProcessor.Core.Utils;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class NotificationMessageProcessor 
        : INotificationProcessor
    {
        public float SafeBodyTemperature { get; set; }

        private readonly ISendMailService _sendMailService;
        public NotificationMessageProcessor(ISendMailService sendMail)
        {
            this._sendMailService = sendMail;
        }

        public Task ProcessAsync(string source, ILogger logger)
        {
            if (!string.IsNullOrWhiteSpace(source))
            {
                var target = MessageConverter.GetMessageType<MailContentData>(source);

                if (target != null)
                    this._sendMailService.SendMailAsync(target, logger);
            }

            return Task.CompletedTask;
        }
    }
}
