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
        public IEnumerable<CompanyTotalScanResult> GetTotalScansByCompany(
            QueryTotalScanParam param)
        {
            var result = (from cd in _thermoDataContext.Company_Device
                          join ar in _thermoDataContext.AttendanceRecord on cd.DeviceId equals
                          ar.DeviceId
                          where ar.TimeStamp >= param.StartDate && ar.TimeStamp <= param.EndDate
                          group cd by cd.CompanyId into g
                          select new CompanyTotalScanResult
                          {
                              CompanyId = g.Key,
                              TotalScans = g.Count()
                          }).ToList();

            if (result != null)
                return result;

            return null;
        }

         public IEnumerable<AbnornormalScanResult> GetTotalAbnormalScanByCompany(
            QueryTotalScanParam param)
        {
            var result = (from cd in _thermoDataContext.Company_Device
                          join ar in _thermoDataContext.AttendanceRecord on cd.DeviceId equals
                          ar.DeviceId
                          where ar.TimeStamp >= param.StartDate && ar.TimeStamp <= param.EndDate
                          && ar.BodyTemperature > param.TemperatureMax
                          group cd by cd.CompanyId into g
                          select new AbnornormalScanResult
                          {
                              CompanyId = g.Key,
                              TotalAbnormalScan = g.Count()
                          }).ToList();

            if (result != null)
                return result;

            return null;
        }

        public IEnumerable<string> GetRecipientsByCompanyId(int companyId)
        {
            var result = from u in _thermoDataContext.Users
                         join er in _thermoDataContext.EmailAlertRecipient on u.Nid equals er.UserId
                         where er.CompanyId == companyId
                         select u.Email;

            if (result != null && result.Any())
            {
                return result.ToList();
            }
            return Enumerable.Empty<string>();
        }
    }    
}
