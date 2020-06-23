using System.Collections.Generic;

namespace Service.ThermoDataModel.Configuration
{
    public class ThermoRestConfiguration
    {
        public IEnumerable<TargetDevice> TargetDevices { get; set; }
    }

    public class TargetDevice
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string BaseUrl { get; set; }

        public string PersonelUrl => "/";

        public string AttendanceUrl => "/api/v1/face/queryAttendRecord";

        public string CheckPointFileName { get; set; }

    }
}
