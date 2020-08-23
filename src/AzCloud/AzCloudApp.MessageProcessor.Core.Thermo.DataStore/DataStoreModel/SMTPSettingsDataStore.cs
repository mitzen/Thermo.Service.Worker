using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class SMTPSettingsDataStore
    {
        [Key]
        public int Nid { get; set; }

        public int Company { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public string FromEmail { get; set; }

        public int Port { get; set; }

    }
}
