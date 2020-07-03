using System;

namespace Service.ThermoDataModel.Heartbeat
{
    public class HeartbeatMessage
    {
        public string ThermoDeviceId { get; set; }

        public string MessageDescription { get; set; }

        public DateTime Timestamp { get; set; }

    }
}
