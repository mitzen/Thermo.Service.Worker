using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Thermo.Web.WebApi.Model.SMTPModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util
{
    public static class SMTPSettingsConverter
    {
        public static void UpateModel(ref SMTPSettingsDataStore source, NewSMTPRequest target)
        {
            source.Company = target.Company;
            source.FromEmail = target.FromEmail;
            source.HostName = target.HostName;
            source.Password = target.Password;
            source.Port = target.Port;
            source.Username = target.Username;
        }

        public static SMTPSettingsDataStore ToModel(this NewSMTPRequest target)
        {
            var source = new SMTPSettingsDataStore();
            source.Company = target.Company;
            source.FromEmail = target.FromEmail;
            source.HostName = target.HostName;
            source.Password = target.Password;
            source.Port = target.Port;
            source.Username = target.Username;
            return source;
        }
    }
}
