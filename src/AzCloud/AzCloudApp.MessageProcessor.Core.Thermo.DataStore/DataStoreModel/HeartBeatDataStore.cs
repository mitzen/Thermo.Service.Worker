using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class HeartBeatDataStore
    {
        [Key]
        public int Nid { get; set; }

        public string DeviceId { get; set; }

        public string Status { get; set; }

        public DateTime? Timestamp { get; set; }

    }
}
