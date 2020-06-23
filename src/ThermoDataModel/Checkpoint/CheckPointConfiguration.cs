using System;

namespace Service.ThermoDataModel.Checkpoint
{
    public class CheckPointConfiguration
    {
        public double SequenceId { get; set; }

        public DateTime LastUpdate { get; set; }

        public double   LastSequence { get; set; }
    }
}
