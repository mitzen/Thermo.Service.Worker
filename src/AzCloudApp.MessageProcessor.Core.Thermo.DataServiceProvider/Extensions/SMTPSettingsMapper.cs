using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System.Collections.Generic;
using Thermo.Web.WebApi.Model.SMTPModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Extensions
{
    public static class SMTPSettingsMapper
    {

        public static SmtpGetResponse MapTo(this SMTPSettingsDataStore source)
        {
            if (source != null)
            {
                var target = new SmtpGetResponse();
                target.Company = source.Company;
                target.FromEmail = source.FromEmail;
                target.HostName = source.HostName;
                target.Nid = source.Nid;
                target.Password = source.Password;
                target.Port = source.Port;
                target.Username = source.Username;

                return target;
            }
            return null;
        }

        public static IEnumerable<SmtpGetResponse> MapTo(this IEnumerable<SMTPSettingsDataStore> source)
        {
            var list = new List<SmtpGetResponse>();

            foreach (var item in source)
            {
                list.Add(item.MapTo());
            }

            return list;
        }
    }
}
