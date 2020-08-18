using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.MessageBusServiceProvider.Converters;
using Service.MessageBusServiceProvider.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.EmailSummary
{
    public class EmailSummaryServiceHandler : ISummaryServiceHandler
    {
        private ServiceBusConfiguration _notificationServiceBusConfiguration;
        private readonly ISummaryEmailProviderDataProcessor _dataProcessor;
        private readonly ISummaryMailContentParser _summaryMailContentParser;
        private readonly EmailSummaryConfiguration _emailSummaryConfiguration;

        public EmailSummaryServiceHandler(IOptions<ServiceBusConfiguration> notificationServiceBusOption,
            IOptions<EmailSummaryConfiguration> emailSummaryOption,
            ISummaryEmailProviderDataProcessor dataProcessor,
            ISummaryMailContentParser parser)
        {
            _dataProcessor = dataProcessor;
            _notificationServiceBusConfiguration = notificationServiceBusOption.Value;
            _summaryMailContentParser = parser;
            _emailSummaryConfiguration = emailSummaryOption.Value;
        }

        public async Task ProcessMessage(ILogger logger)
        {
            logger.LogInformation($"NotificationSummaryProcessor : Getting summary email list of notifications." +
                $"Connecion:{_notificationServiceBusConfiguration.ServiceBusConnection} and queuename: {_notificationServiceBusConfiguration.QueueName}");

            var queryParameter = CreateParam();
            var totalScan = ComputeTotalSummaryScans(queryParameter, logger);

            if (totalScan != null)
            {
                logger.LogInformation($"Group by company count : {totalScan?.Count()}");

                foreach (var item in totalScan)
                {
                    logger.LogInformation($"COMPUTE: {item.CompanyId},{item.TotalScans},{item.TotalAbnormalScan}");
                }


                var _messageSender = MessageBusServiceFactory.CreateServiceBusMessageSender(_notificationServiceBusConfiguration, logger);

                foreach (var item in totalScan)
                {
                    logger.LogInformation($"Total daily scan - {item.TotalScans}.");

                    var mailParam = new ParseEmailParam(item.CompanyId, item.TotalScans);
                    mailParam.Recipients = _dataProcessor.GetRecipientsByCompanyId(item.CompanyId);
                    mailParam.TotalAbnormalDetected = item.TotalAbnormalScan;

                    var mailData = _summaryMailContentParser.CreateSummaryEmailAlertMessage(mailParam, logger);

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
            else
            {
                logger.LogInformation($"Unable to find any summary record matches in the database.{DateTime.Now}");
            }
        }

        private IEnumerable<CompanyTotalScanResult> ComputeTotalSummaryScans(QueryTotalScanParam queryParam, ILogger logger)
        { 
            logger.LogInformation($"ComputeTotalSummaryScans routine. Start date : {queryParam.StartDate}, End Date: { queryParam.EndDate }, Max: { queryParam.TemperatureMax }");

            var totalScan = GetTotalScanRecord(queryParam)?.ToList();
          
            if (totalScan != null)
            {
                var totalAbnormalScan = GetAbnormalScanRecord(queryParam);
                logger.LogInformation($"Abnormal : {totalAbnormalScan?.Count()}");

                for (int i = 0; i < totalScan.Count(); i++)
                {
                    totalScan[i].TotalAbnormalScan = GetAbScanCountByCompanyId(sourceItem.CompanyId, totalAbnormalScan);
                }
            }
            return totalScan;
        }

        private int GetAbScanCountByCompanyId(int targetCompanyId, IEnumerable<AbnornormalScanResult> totalAbnormalScan)
        {
            return totalAbnormalScan.Where(x => x.CompanyId == targetCompanyId).Select(x => x.TotalAbnormalScan).FirstOrDefault();
        }

        private QueryTotalScanParam CreateParam()
        {
            DateTime endDate = DateTime.MinValue;
          
            if (!DateTime.TryParse(_emailSummaryConfiguration.TargetDate, out endDate))
            {
                endDate = DateTime.Now;
            }

            var startDate = endDate.AddDays(-1);

            return new QueryTotalScanParam
            {
                StartDate = startDate,
                EndDate = endDate,
                TemperatureMax = _emailSummaryConfiguration.MaxTemperature,
            };
        }

        private IEnumerable<CompanyTotalScanResult> GetTotalScanRecord(QueryTotalScanParam param)
        {
            return _dataProcessor.GetTotalScansByCompany(param);
        }

        private IEnumerable<AbnornormalScanResult> GetAbnormalScanRecord(QueryTotalScanParam param)
        {
            return _dataProcessor.GetTotalAbnormalScanByCompany(param);
        }
    }
}
