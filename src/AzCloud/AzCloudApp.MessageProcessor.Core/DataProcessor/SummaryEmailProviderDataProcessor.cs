using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
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
        public IEnumerable<CompanyEmailSendSummary> GetSummaryEmailSentGroupByCompany()
        {
            //var result = (from cd in _thermoDataContext.Company_Device
            //              join er in _thermoDataContext.EmailAlertRecipient on cd.CompanyId equals
            //              er.CompanyId
            //              where cd.DeviceId == deviceId
            //              select new
            //              {
            //                 EmailAddress = er.EmailAddress
            //              }).ToList();

            //if (result != null)
            //    return result.Select(x => x.EmailAddress).ToList();

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
        IEnumerable<CompanyEmailSendSummary> GetSummaryEmailSentGroupByCompany();

        IEnumerable<string> GetRecipientsByCompanyId(int companyId);
    }

    public class CompanyEmailSendSummary
    {
        public int CompanyId { get; set; }

        public int TotalSent { get; set; }

    }
}
