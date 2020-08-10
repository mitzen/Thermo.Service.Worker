using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.ThermoDataModel.Configuration;
using Service.ThermoDataModel.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface ISendMailService
    {
        Task SendMailAsync(MailContentData record, ILogger logger);
    }

    public class SendMailService : ISendMailService
    {
        private NotificationConfiguration _notificationConfiguration;

        public SendMailService(IOptions<NotificationConfiguration> notificationConfiguration)
        {
            _notificationConfiguration = notificationConfiguration.Value;
        }

        public async Task SendMailAsync(MailContentData record, ILogger logger)
        {
            logger.LogInformation($"Sending email.");

            var apiKey = _notificationConfiguration.ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(record.MailInfo.Sender, record.MailInfo.SenderName);

            List<EmailAddress> tos = new List<EmailAddress>();

            foreach (var recipient in record.MailInfo.Recipients)
            {
                tos.Add(new EmailAddress(recipient));
            }

            var subject = record.MailInfo.Subject;
            var htmlContent = record.MailInfo.ContentBody;
            var displayRecipients = false;

            var mssage = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos,
                subject, string.Empty, htmlContent, false);

            var response = await client.SendEmailAsync(mssage);

            logger.LogInformation($"Mail message sent at {DateTime.Now}");

        }
    }
}