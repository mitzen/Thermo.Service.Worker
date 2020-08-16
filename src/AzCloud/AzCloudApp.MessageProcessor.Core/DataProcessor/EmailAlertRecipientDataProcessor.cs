using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System.Collections.Generic;
using System.Linq;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class EmailAlertRecipientDataProcessor : IEmailAlertRecipientDataProcessor
    {
        private readonly ThermoDataContext _thermoDataContext;
        public EmailAlertRecipientDataProcessor(ThermoDataContext context)
        {
            _thermoDataContext = context;
        }
        public IEnumerable<string> GetEmailByDeviceId(string deviceId)
        {
            var result = (from cd in _thermoDataContext.Company_Device
                          join er in _thermoDataContext.EmailAlertRecipient on cd.CompanyId equals
                          er.CompanyId
                          join u in _thermoDataContext.Users on er.CompanyId equals
                         u.CompanyId
                          where cd.DeviceId == deviceId
                          select new
                          {
                             EmailAddress = u.Username
                          }).ToList();

            if (result != null)
                return result.Select(x => x.EmailAddress).ToList();

            return null;
        }
    }

    public interface IEmailAlertRecipientDataProcessor
    {
        IEnumerable<string> GetEmailByDeviceId(string deviceId);
    }
}
