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
            SummaryParam param)
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

         public IEnumerable<CompanyTotalScanResult> GetTotalAbnormalScanByCompany(
            SummaryParam param)
        {
            var result = (from cd in _thermoDataContext.Company_Device
                          join ar in _thermoDataContext.AttendanceRecord on cd.DeviceId equals
                          ar.DeviceId
                          where ar.TimeStamp >= param.StartDate && ar.TimeStamp <= param.EndDate
                          && ar.BodyTemperature > param.TemperatureMax
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

        public int GetAbnormalScanCountByCompanyId(int companyId, DateTime start, DateTime end)
        {
            //var count = from ar in _thermoDataContext.AttendanceRecord
            //            where ar.TimeStamp >= start && ar.TimeStamp <= end
            //            && ar.DeviceId = companyId;
            return 0;
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
        IEnumerable<CompanyTotalScanResult> GetTotalScansByCompany(SummaryParam param);

        IEnumerable<string> GetRecipientsByCompanyId(int companyId);

        IEnumerable<CompanyTotalScanResult> GetTotalAbnormalScanByCompany(
            SummaryParam param);
    }

    public class CompanyTotalScanResult
    {
        public int CompanyId { get; set; }

        public int TotalScans { get; set; }
    }

    public class SummaryParam
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double TemperatureMax { get; set; }
    }
}
