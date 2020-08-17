using System;
using AzCloudApp.MessageProcessor.Core.EmailSummary;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzCloudApp.EmailSummary.Function
{
    public class EmailSummaryFunction
    {
        private readonly INotificationSummaryProcessor _emailSummaryProcessor;
        public EmailSummaryFunction(INotificationSummaryProcessor emailSummaryProcessor)
        {
            _emailSummaryProcessor = emailSummaryProcessor;
        }

        [FunctionName("EmailSummaryFunction")]
        public void Run([TimerTrigger("%EmailScheduleTriggerTime%")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Starting EmailSummaryNotificationFunction:{DateTime.Now}");
            _emailSummaryProcessor.ProcessMessage(log);
        }
    }
}
