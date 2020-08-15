using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.MessageBusServiceProvider.Converters;
using Service.MessageBusServiceProvider.Queue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.EmailSummary
{
    public class NotificationSummaryProcessor : INotificationSummaryProcessor
    {
        private ServiceBusConfiguration _notificationServiceBusConfiguration;
        private readonly ISummaryEmailProviderDataProcessor _dataProcessor;
        private readonly ISummaryMailContentParser _summaryMailContentParser;

        public NotificationSummaryProcessor(IOptions<ServiceBusConfiguration> notificationServiceBusOption, ISummaryEmailProviderDataProcessor dataProcessor,
            ISummaryMailContentParser parser)
        {
            _dataProcessor = dataProcessor;
            _notificationServiceBusConfiguration = notificationServiceBusOption.Value;
            _summaryMailContentParser = parser;
        }

        public async Task ProcessMessage(ILogger logger)
        {
            logger.LogInformation($"NotificationSummaryProcessor : Getting summary email list of notifications." +
                $"Connecion:{_notificationServiceBusConfiguration.ServiceBusConnection} and queuename: {_notificationServiceBusConfiguration.QueueName}");

            var _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_notificationServiceBusConfiguration, logger);

            var summaryResut = _dataProcessor.GetSummaryEmailSentGroupByCompany();

            foreach (var item in summaryResut)
            {
                // Parse email info //
                var mailParam = new EmailSummaryParam(item.CompanyId, item.TotalSent);
                mailParam.Recipients = _dataProcessor.GetRecipientsByCompanyId(item.CompanyId);

                var mailData = _summaryMailContentParser.CreateSummaryEmailAlertMessage(
                    mailParam, logger);

                if (mailData != null)
                {
                    var messgeInstance = MessageConverter.Serialize(mailData);
                    await _messageSender.SendMessagesAsync(messgeInstance);

                    logger.LogInformation($"Summary {item.CompanyId} data to notification queue.");
                }
                else
                {
                    logger.LogInformation($"No notifcation sent to queue. {DateTime.Now}");
                }
            }
        }
    }

    public interface INotificationSummaryProcessor
    {
        Task ProcessMessage(ILogger logger);
    }

    public class EmailSummaryParam
    {
        public int CompanyId { get; set; }

        public int TotalScans { get; set; }

        public int TotalAbnormalDetected { get; set; }

        public IEnumerable<string> Recipients { get; set; }

        public EmailSummaryParam(int companyId, int emailCount)
        {
            CompanyId = companyId;
            TotalScans = emailCount;
        }
    }
}
