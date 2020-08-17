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
            var result = _thermoDataContext.Users.Where(x => x.CompanyId == companyId)
                .Select(x => x.Email).ToList();

            if (result != null && result.Any())
            {
                return result;
            }
            return Enumerable.Empty<string>();
        }
    }

    public interface ISummaryEmailProviderDataProcessor
    {
        IEnumerable<CompanyTotalScanResult> GetTotalScansByCompany(QueryTotalScanParam param);

        IEnumerable<string> GetRecipientsByCompanyId(int companyId);

        IEnumerable<AbnornormalScanResult> GetTotalAbnormalScanByCompany(
            QueryTotalScanParam param);
    }

    public class CompanyTotalScanResult
    {
        public int CompanyId { get; set; }

        public int TotalScans { get; set; }

        public int TotalAbnormalScan { get; set; }
    }

    public class AbnornormalScanResult
    {
        public int CompanyId { get; set; }

        public int TotalAbnormalScan { get; set; }
    }


    public class QueryTotalScanParam
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double TemperatureMax { get; set; }
    }
}
