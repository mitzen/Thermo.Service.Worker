using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Thermo.Web.WebApi.Model.SMTPModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util
{
    public static class SMTPSettingsConverter
    {
        public static void UpateModel(ref SMTPSettingsDataStore source, NewSMTPRequest target)
        {
            source.Host = target.Host;
            source.Name = target.Name;
            source.Username = target.Username;
            source.Password = target.Password;
            source.Port = target.Port;
        }

        public static SMTPSettingsDataStore ToModel(this NewSMTPRequest target)
        {
            var source = new SMTPSettingsDataStore();
            source.Host = target.Host;
            source.Name = target.Name;
            source.Username = target.Username;
            source.Password = target.Password;
            source.Port = target.Port;
            return source;
        }
    }
}
