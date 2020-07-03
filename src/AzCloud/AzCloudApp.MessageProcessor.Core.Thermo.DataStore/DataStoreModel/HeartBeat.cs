using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class Heartbeat
    {
        [Key]
        public int Id { get; set; }

        public string ThermoDeviceId { get; set; }

        public string MessageDescription { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
