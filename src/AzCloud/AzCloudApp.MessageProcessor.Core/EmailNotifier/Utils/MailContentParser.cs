using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Models;
using System;
using System.Collections.Generic;

namespace AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils
{
    public class MailContentParser : IMailContentParser
    {
        private readonly IEmailAlertRecipientDataProcessor _emailAlertRecipientDataProcessor;
        private TemperatureFilterConfiguration _temperatureFilterConfiguration;

        public MailContentParser(IOptions<TemperatureFilterConfiguration> temperatureOption, IEmailAlertRecipientDataProcessor emailAlertRecipientDataProcessor)
        {
            _emailAlertRecipientDataProcessor = emailAlertRecipientDataProcessor;
            _temperatureFilterConfiguration = temperatureOption.Value;
        }

        public MailContentData CreateMailMessage(EmailInfoParameter infoParameter, ILogger logger)
        {
            var mailData = new MailContentData();
            mailData.MailInfo = new MailInfo();

            logger.LogInformation("Getting sender/recipient email address.");
            var recipients = GetEmailAddress(infoParameter.DeviceId);

            if (recipients == null)
                throw new ArgumentNullException("Recipients is null, please ensure recipient are setup properly.");

            mailData.MailInfo.Recipients = recipients;
            mailData.MailInfo.ContentBody = ApplyTextReplacement(infoParameter);
            mailData.MailInfo.Sender = _temperatureFilterConfiguration.Sender;
            mailData.MailInfo.SenderName = _temperatureFilterConfiguration.SenderName;

            return mailData;
        }

        private string ApplyTextReplacement(EmailInfoParameter mailInfo)
        {
            mailInfo.EmailMessage = mailInfo.EmailMessage.ReplaceContent("###THERO_UNIT###", mailInfo.DeviceId).ReplaceContent("###INCIDENT_DATE###", mailInfo.Timestamp.ToString()).ReplaceContent("###TEMPERATURE###", mailInfo.TemperatureRegistered.ToString()).ReplaceContent("###IMG###", mailInfo.Image);
            return mailInfo.EmailMessage;
        }

        private IEnumerable<string> GetEmailAddress(string deviceId)
        {
            return _emailAlertRecipientDataProcessor.GetEmailByDeviceId(deviceId);
        }
    }

    public interface IMailContentParser
    {
        MailContentData CreateMailMessage(EmailInfoParameter emaiInfoParameter, ILogger logger);
    }

    public class EmailInfoParameter
    {
        public string DeviceId { get; set; }

        public string EmailMessage { get; set; }

        public string Location { get; set; }

        public double TemperatureRegistered { get; set; }

        public string Image { get; set; }

        public DateTime? Timestamp { get; set; }

        public EmailInfoParameter(string deviceId, string emailMessage)
        {
            DeviceId = deviceId;
            EmailMessage = emailMessage;
        }
    }
}
