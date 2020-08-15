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

            logger.LogInformation($"Parsing email summary content. TC:{ param.TotalAbnormalDetected }, TA:{ param.TotalAbnormalDetected }");

            if (param.Recipients == null && !param.Recipients.Any())
                throw new ArgumentNullException("Recipients is null, please ensure recipient are setup properly.");

            mailData.MailInfo.Recipients = param.Recipients;
            mailData.MailInfo.ContentBody = ApplyTextReplacement(_temperatureFilterConfiguration.EmailTemplate, param);
            mailData.MailInfo.Sender = _temperatureFilterConfiguration.Sender;
            mailData.MailInfo.SenderName = _temperatureFilterConfiguration.SenderName;

            logger.LogInformation($"Parsed content : mailData.MailInfo.ContentBody");
            return mailData;
        }
        private string ApplyTextReplacement(string emailTemplate, EmailSummaryParam param)
        {
            var replacedContent = emailTemplate.ReplaceContent("###TOTAL_SCANS###", param.TotalScans.ToString()).ReplaceContent("###ABNORMAL_SCANS###", param.TotalAbnormalDetected.ToString());
            
            return replacedContent;
        }
    }

    public interface ISummaryMailContentParser
    {
        MailContentData CreateSummaryEmailAlertMessage(EmailSummaryParam param, ILogger logger);
    }
}
