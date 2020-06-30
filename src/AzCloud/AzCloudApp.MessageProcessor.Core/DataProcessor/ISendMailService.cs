using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface ISendMailService
    {
        Task SendMailAsync(MailContentData record, ILogger logger);
    }

    public class SendMailService : ISendMailService
    {
        private ILogger<SendMailService> _logger;
        private NotificationConfiguration _notificationConfiguration;

        public SendMailService(ILogger<SendMailService> logger, IOptions<NotificationConfiguration> notificationConfiguration)
        {
            _logger = logger;
            _notificationConfiguration = notificationConfiguration.Value;
        }

        public async Task SendMailAsync(MailContentData record, ILogger logger)
        {
            this._logger.LogInformation($"Sending email : {this._notificationConfiguration.SmtpServer}");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(record.MailInfo.Sender);

            foreach (var recipient in record.MailInfo.Recipients)
            {
                message.To.Add(new MailAddress(recipient));
            }

            message.Subject = record.MailInfo.Subject;
            message.IsBodyHtml = true;
            message.Body = record.MailInfo.Body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Port = this._notificationConfiguration.Port;
            smtpClient.Host = this._notificationConfiguration.SmtpServer; //for gmail host  
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(
                this._notificationConfiguration.Username,
                this._notificationConfiguration.Password
             );
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.SendAsync(message, new object());
        }
    }
}