using System;

namespace Service.ThermoDataModel.Checkpoint
{
    public class CheckPointConfiguration
    {
        public DateTime LastUpdate { get; set; }

        public int LastSequence { get; set; }
    }
}
