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
        private readonly SendGridClient _client;
        public SendMailService(IOptions<NotificationConfiguration> notificationConfiguration)
        {
            _notificationConfiguration = notificationConfiguration.Value;
            _client = new SendGridClient(_notificationConfiguration.ApiKey);
        }

        public async Task SendMailAsync(MailContentData record, ILogger logger)
        {
            logger.LogInformation($"Sending email. {DateTime.Now}");

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
                subject, string.Empty, htmlContent, displayRecipients);

            var response = await _client.SendEmailAsync(mssage);

            logger.LogInformation($"Mail message sent result {response.StatusCode} at {DateTime.Now}");

        }
    }
}