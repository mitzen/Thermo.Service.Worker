using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class EmailAlertRecipientDataStore
    {
        [Key]
        public long Nid { get; set; }

        public int CompanyId { get; set; }

        public string EmailAddress { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
