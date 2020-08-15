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
        public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Starting EmailSummaryNotificationFunction:{DateTime.Now}");
            _emailSummaryProcessor.ProcessMessage(log);
        }
    }
}
