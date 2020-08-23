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
        private const string RecipientNullErrorMessage = "Recipients is null, please ensure recipient are setup properly.";
        private EmailSummaryConfiguration _temperatureFilterConfiguration;
        public SummaryMailContentParser(IOptions<EmailSummaryConfiguration> temperatureOption)
        {
            _temperatureFilterConfiguration = temperatureOption.Value;
        }

        public MailContentData CreateSummaryEmailAlertMessage(ParseEmailParam param, ILogger logger)
        {
            var mailData = new MailContentData();
            mailData.MailInfo = new MailInfo();

            if (param.Recipients == null && !param.Recipients.Any())
                throw new ArgumentNullException(RecipientNullErrorMessage);

            mailData.MailInfo.Recipients = param.Recipients;
            mailData.MailInfo.Subject = _temperatureFilterConfiguration.Subject;
            mailData.MailInfo.ContentBody = ApplyTextReplacement(_temperatureFilterConfiguration.EmailTemplate, param);
            mailData.MailInfo.Sender = _temperatureFilterConfiguration.Sender;
            mailData.MailInfo.SenderName = _temperatureFilterConfiguration.SenderName;

            return mailData;
        }
        private string ApplyTextReplacement(string emailTemplate, ParseEmailParam param)
        {
            var replacedContent = emailTemplate.ReplaceContent("###TOTAL_SCANS###", param.TotalScans.ToString()).ReplaceContent("###ABNORMAL_SCANS###", param.TotalAbnormalDetected.ToString());
            
            return replacedContent;
        }
    }

    public interface ISummaryMailContentParser
    {
        MailContentData CreateSummaryEmailAlertMessage(ParseEmailParam param, ILogger logger);
    }
}
