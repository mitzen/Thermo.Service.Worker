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
        private const string ImagePlaceHolderReplacement = "###IMG###";
        private const string TemperaturePlaceHolderReplacement = "###TEMPERATURE###";
        private const string IncidentDatePlaceHolderReplacement = "###INCIDENT_DATE###";
        private const string ThermoUnitPlaceHolderReplacement = "###THERO_UNIT###";
        private readonly IEmailAlertRecipientDataProcessor _emailAlertRecipientDataProcessor;
        private TemperatureFilterConfiguration _temperatureFilterConfiguration;

        public MailContentParser(IOptions<TemperatureFilterConfiguration> temperatureOption, 
            IEmailAlertRecipientDataProcessor emailAlertRecipientDataProcessor)
        {
            _temperatureFilterConfiguration = temperatureOption.Value;
            _emailAlertRecipientDataProcessor = emailAlertRecipientDataProcessor;
        }

        public MailContentData CreateTemperatureEmailAlertMessage(EmailTemperatureHitParameter infoParameter, 
            ILogger logger)
        {
            var mailData = new MailContentData();
            mailData.MailInfo = new MailInfo();

            logger.LogInformation("Getting sender/recipient email address.");
            var recipients = GetEmailAddress(infoParameter.DeviceId);

            if (recipients == null)
                throw new ArgumentNullException("Recipients is null, please ensure recipient are setup properly.");

            mailData.MailInfo.Recipients = recipients;
            mailData.MailInfo.Subject = _temperatureFilterConfiguration.Subject;
            mailData.MailInfo.ContentBody = ApplyTextReplacement(infoParameter);
            mailData.MailInfo.Sender = _temperatureFilterConfiguration.Sender;
            mailData.MailInfo.SenderName = _temperatureFilterConfiguration.SenderName;
            return mailData;
        }
        
        private string ApplyTextReplacement(EmailTemperatureHitParameter mailInfo)
        {
            mailInfo.EmailMessage = mailInfo.EmailMessage.ReplaceContent(ThermoUnitPlaceHolderReplacement, 
                mailInfo.DeviceId).ReplaceContent(IncidentDatePlaceHolderReplacement, mailInfo.Timestamp.ToString()).ReplaceContent(TemperaturePlaceHolderReplacement, mailInfo.TemperatureRegistered.ToString()).ReplaceContent(ImagePlaceHolderReplacement, mailInfo.Image);
            return mailInfo.EmailMessage;
        }

        private IEnumerable<string> GetEmailAddress(string deviceId)
        {
            return _emailAlertRecipientDataProcessor.GetEmailByDeviceId(deviceId);
        }
    }
}
