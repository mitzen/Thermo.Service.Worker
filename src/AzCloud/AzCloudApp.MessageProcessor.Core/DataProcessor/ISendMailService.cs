using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface ISendMailService
    {
        Task SendMailAsync(ThermoMailContent record, ILogger logger);
    }

    public class SendMailService : ISendMailService
    {
        private ILogger _logger;
        private NotificationConfiguration _notificationConfiguration;

        public SendMailService(IOptions<NotificationConfiguration> notificationConfiguration)
        {
            _notificationConfiguration = notificationConfiguration.Value;
        }

        public async Task SendMailAsync(ThermoMailContent mailContent, ILogger logger)
        {
            logger.LogInformation("SendMailService : constructing message");
            MailMessage message = ConstructMailMessage(mailContent);
            await SendMailOut(message);
            logger.LogInformation($"SendMailService : message delivered to {this._notificationConfiguration.SmtpServer} {this._notificationConfiguration.Port}");
        }

        private async Task SendMailOut(MailMessage message)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = this._notificationConfiguration.Port;
            smtp.Host = this._notificationConfiguration.SmtpServer; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(
                this._notificationConfiguration.Username,
                this._notificationConfiguration.Password
             );

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            await smtp.SendMailAsync(message);
        }

        private static MailMessage ConstructMailMessage(ThermoMailContent mailContent)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(mailContent!.MailingInfoData.SenderAddress);

            foreach (var targetRecipient in mailContent.MailingInfoData.Recipients)
            {
                message.To.Add(new MailAddress(targetRecipient));
            }

            message.Subject = mailContent.MailingInfoData.Subject;
            message.IsBodyHtml = mailContent.MailingInfoData.HtmlContent != null;
            message.Body = mailContent.MailingInfoData.Content;
            return message;
        }
    }
}