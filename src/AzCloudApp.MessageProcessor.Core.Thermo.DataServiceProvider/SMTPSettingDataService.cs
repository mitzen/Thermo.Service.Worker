using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util;
using Thermo.Web.WebApi.Model.SMTPModel;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Extensions;
using System;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider
{
    public class SMTPSettingDataService
    {
        private readonly ThermoDataContext _thermoDataContext;

        public SMTPSettingDataService(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        public virtual Task<int> SaveSmtpSettings(NewSMTPRequest source)
        {
            ValidateOptions(source);

            var targetRecord = GetSmtpSettingsByCompanyId(source.Company);

            if (targetRecord == null)
            {
                this._thermoDataContext.SMTPSettings.Add(source.ToModel());
                return this._thermoDataContext.SaveChangesAsync();
            }

            return Task.FromResult(-1);

        }

        public virtual Task<int> UpdateSmtpSettings(NewSMTPRequest source)
        {
            ValidateOptions(source);

            var targetRecord = GetSmtpSettingsByCompanyId(source.Company);

            if (targetRecord != null)
            {
                SMTPSettingsConverter.UpateModel(ref targetRecord, source);
                return this._thermoDataContext.SaveChangesAsync();
            }

            return Task.FromResult(-1);
        }

        private void ValidateOptions(NewSMTPRequest source)
        {
            if (source == null)
                throw new ArgumentNullException();
            if (source.Company == 0)
                throw new ArgumentException("Company is required");
            if (source.Port == 0)
                throw new ArgumentException("Port  is required");
            if (string.IsNullOrWhiteSpace(source.HostName))
                throw new ArgumentException("Hostname is required");
        }

        public virtual SmtpGetResponse GetSmtpSettingsByCompanyIdAsync(int? source)
        {
            if (source.HasValue)
            {
                var result = this._thermoDataContext.SMTPSettings.Where(x => x.Company == source.Value).FirstOrDefault();

                return result.MapTo();
            }

            return null;
        }

        public virtual IEnumerable<SmtpGetResponse> GetAllSmtpSettings()
        {
            return this._thermoDataContext.SMTPSettings.MapTo().ToList();
        }

        public virtual Task<int> DeleteSmtpSettingsAsync(SMTPDeleteRequest source)
        {
            var isRecordDelete = false;

            if (source == null)
                 return Task.FromResult(-1);

            var usersToRemove = DataTypeHelper.ConvertToIntegerArray(source?.Targets);

            foreach (var targetUserId in usersToRemove)
            {
                var targetRecord = GetSmtpById(targetUserId);

                if (targetRecord != null)
                {
                    isRecordDelete = true;
                    _thermoDataContext.SMTPSettings.Remove(targetRecord);
                }
            }

            if (isRecordDelete)
                return _thermoDataContext.SaveChangesAsync();
             
            return Task.FromResult(-1);
        }

        private SMTPSettingsDataStore GetSmtpById(long? source)
        {
            if (source.HasValue)
            {
                return this._thermoDataContext.SMTPSettings.Where(x => x.Nid == source.Value).FirstOrDefault();
            }

            return null;
        }

        private SMTPSettingsDataStore GetSmtpSettingsByCompanyId(int? username)
        {
            if (username.HasValue)
            {
                return this._thermoDataContext.SMTPSettings.Where(x => x.Company == username.Value).FirstOrDefault();
            }

            return null;
        }
    }
}
