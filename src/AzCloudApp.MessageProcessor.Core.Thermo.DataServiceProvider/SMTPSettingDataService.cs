using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util;
using Thermo.Web.WebApi.Model.SMTPModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider
{
    public class SMTPSettingDataService
    {
        private readonly ThermoDataContext _thermoDataContext;

        public SMTPSettingDataService(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        public virtual Task<int> SaveSMTPAsync(NewSMTPRequest source)
        {
            var targetRecord = GetSingleSMTPSettingsAsync(source.Name);

            if (targetRecord == null)
            {
                this._thermoDataContext.SMTPSettings.Add(source.ToModel());
            }
            else
            {
                SMTPSettingsConverter.UpateModel(ref targetRecord, source);
            }

            return this._thermoDataContext.SaveChangesAsync();
        }

        public virtual SMTPSettingsDataStore GetSingleSMTPSettingsAsync(int? source)
        {
            if (source.HasValue)
            {
                return this._thermoDataContext.SMTPSettings.Where(x => x.Nid == source.Value).FirstOrDefault();
            }

            return null;
        }

        public virtual SMTPSettingsDataStore GetSingleSMTPSettingsAsync(string source)
        {
            if (source != null)
            {
                return this._thermoDataContext.SMTPSettings.Where(x => x.Name == source).FirstOrDefault();
            }

            return null;
        }
        public virtual SMTPSettingsDataStore GetSingleSMTPSettingsByIdAsync(int? source)
        {
            if (source.HasValue)
            {
                return this._thermoDataContext.SMTPSettings.Where(x => x.Nid == source.Value).FirstOrDefault();
            }

            return null;
        }

        public virtual IEnumerable<SMTPSettingsDataStore> GetSingleSMTPSettings()
        {
            return this._thermoDataContext.SMTPSettings;
        }

        public virtual Task<int> DeleteSMTPSettingsAsync(SMTPDeleteRequest source)
        {
            var targetRecord = GetSingleSMTPSettingsByIdAsync(source.Nid);

            if (targetRecord == null)
            {
                this._thermoDataContext.SMTPSettings.Remove(targetRecord);
            }

            return this._thermoDataContext.SaveChangesAsync();
        }
    }
}
