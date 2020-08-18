using System;
using AzCloudApp.MessageProcessor.Core.EmailSummary;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzCloudApp.EmailSummary.Function
{
    public class EmailSummaryFunction
    {
        private readonly ISummaryServiceHandler _emailSummaryProcessor;
        public EmailSummaryFunction(ISummaryServiceHandler emailSummaryProcessor)
        {
            _emailSummaryProcessor = emailSummaryProcessor;
        }

        [FunctionName("EmailSummaryFunction")]
        public void Run([TimerTrigger("%EmailScheduleTriggerTime%")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Starting EmailSummaryNotificationFunction. v1aa:{DateTime.Now}");
            _emailSummaryProcessor.ProcessMessage(log);
        }
    }
}
