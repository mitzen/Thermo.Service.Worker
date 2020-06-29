using AzCloudApp.MessageProcessor.Core.Utils;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class NotificationMessageProcessor : INotificationProcessor
    {
        public float SafeBodyTemperature { get; set; }

        private readonly ISendMailService _sendMailService;
        public NotificationMessageProcessor(ISendMailService sendMail)
        {
            this._sendMailService = sendMail;
        }

        public Task<int> ProcessAsync(string source, ILogger logger)
        {
            logger.LogInformation($"Starting NotificationMessageProcessor. Mail service: {_sendMailService.GetType().Name}.");

            if (!string.IsNullOrWhiteSpace(source))
            {
                var target = MessageConverter.GetMessageType<AttendanceRecord>(source);
                if (target != null)
                {
                    var mailMessageContent = new ThermoMailContent(new MailingInfo(), target); 
                    this._sendMailService.SendMailAsync(mailMessageContent, logger);
                }
            }

            return Task.FromResult(1);
        }
    }
}
