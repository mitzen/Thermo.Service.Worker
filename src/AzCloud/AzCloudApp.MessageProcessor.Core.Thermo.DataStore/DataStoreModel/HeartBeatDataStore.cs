using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class HeartBeatDataStore
    {
        [Key]
        public int Id { get; set; }

        public string ThermoDeviceId { get; set; }

        public string MessageDescription { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
