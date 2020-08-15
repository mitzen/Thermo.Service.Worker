using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class SummaryEmailProviderDataProcessor : ISummaryEmailProviderDataProcessor
    {
        private readonly ThermoDataContext _thermoDataContext;
        public SummaryEmailProviderDataProcessor(ThermoDataContext context)
        {
            _thermoDataContext = context;
        }
        public IEnumerable<CompanyEmailSendSummary> GetSummaryEmailSentGroupByCompany(
            DateTime start, DateTime end)
        {
            var result = (from cd in _thermoDataContext.Company_Device
                          join ar in _thermoDataContext.AttendanceRecord on cd.DeviceId equals
                          ar.DeviceId
                          where ar.TimeStamp >= start && ar.TimeStamp <= end
                          group cd by cd.CompanyId into g
                          select new CompanyEmailSendSummary
                          {
                              CompanyId = g.Key,
                              TotalScans = g.Count()
                          }).ToList();

            if (result != null)
                return result;

            return null;
        }

        public IEnumerable<string> GetRecipientsByCompanyId(int companyId)
        {
            var result = _thermoDataContext.EmailAlertRecipient.Where(x => x.CompanyId == companyId)
                .Select(x => x.EmailAddress).ToList();

            if (result != null && result.Any())
            {
                return result;
            }
            return Enumerable.Empty<string>();
        }
    }

    public interface ISummaryEmailProviderDataProcessor
    {
        IEnumerable<CompanyEmailSendSummary> GetSummaryEmailSentGroupByCompany(DateTime start, DateTime end);

        IEnumerable<string> GetRecipientsByCompanyId(int companyId);
    }

    public class CompanyEmailSendSummary
    {
        public int CompanyId { get; set; }

        public int TotalScans { get; set; }

    }
}
