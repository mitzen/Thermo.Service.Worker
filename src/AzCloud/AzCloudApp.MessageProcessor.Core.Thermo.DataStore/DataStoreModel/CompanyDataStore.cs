using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class CompanyDataStore
    {
        [Key]
        public long Nid { get; set; }
        public string CompanyName { get; set; }

        public string DeviceId { get; set; }

        public Boolean MaskRequired { get; set; }

        public Double MaxTempThreshold { get; set; }

        public Double MinTempThreshold { get; set; }

        public int NoDaysPassDataShowInDays { get; set; }

        public DateTime? TimeStamp { get; set; }

    }
}
