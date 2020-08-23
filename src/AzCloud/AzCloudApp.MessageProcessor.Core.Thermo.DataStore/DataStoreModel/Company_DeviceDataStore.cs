using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class Company_DeviceDataStore
    {
        [Key]
        public long Nid { get; set; }
        public int CompanyId { get; set; }

        public string DeviceId { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}
