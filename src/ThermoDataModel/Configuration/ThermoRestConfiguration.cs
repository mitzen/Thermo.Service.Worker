using System;
using System.Collections.Generic;
using System.Text;

namespace Service.ThermoDataModel.Configuration
{
    public class ThermoRestConfiguration
    {
        public string HostName { get; set; }

        public int Port { get; set; }

        public string BaseUrl { get; set; }

        public string PersonelUrl => "/";

        public string AttendanceUrl => "/api/v1/face/queryAttendRecord";

    }
}
