using System;

namespace Service.ThermoDataModel.Heartbeat
{
    public class HeartbeatMessage : CoreMessage
    {
        public string DeviceId { get; set; }

        public string Status { get; set; }

        public DateTime? Timestamp { get; set; }

    }
}
