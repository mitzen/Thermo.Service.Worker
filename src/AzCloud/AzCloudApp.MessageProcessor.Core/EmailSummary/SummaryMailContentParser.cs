using AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.ThermoDataModel.Models;
using System;
using System.Linq;

namespace AzCloudApp.MessageProcessor.Core.EmailSummary
{
    public class SummaryMailContentParser : ISummaryMailContentParser
    {
        private EmailSummaryConfiguration _temperatureFilterConfiguration;
        public SummaryMailContentParser(IOptions<EmailSummaryConfiguration> temperatureOption)
        {
            _temperatureFilterConfiguration = temperatureOption.Value;
        }

        public MailContentData CreateSummaryEmailAlertMessage(EmailSummaryParam param, ILogger logger)
        {
            var mailData = new MailContentData();
            mailData.MailInfo = new MailInfo();

            logger.LogInformation("Getting sender/recipient email address.");

            if (param.Recipients == null && !param.Recipients.Any())
                throw new ArgumentNullException("Recipients is null, please ensure recipient are setup properly.");

            mailData.MailInfo.Recipients = param.Recipients;
            mailData.MailInfo.ContentBody = ApplyTextReplacement(_temperatureFilterConfiguration.EmailTemplate);
            mailData.MailInfo.Sender = _temperatureFilterConfiguration.Sender;
            mailData.MailInfo.SenderName = _temperatureFilterConfiguration.SenderName;
            return mailData;
        }
        private string ApplyTextReplacement(string emailTemplate)
        {
            return emailTemplate.ReplaceContent("###THERO_UNIT###", "");
        }
    }

    public interface ISummaryMailContentParser
    {
        MailContentData CreateSummaryEmailAlertMessage(EmailSummaryParam param, ILogger logger);
    }
}
