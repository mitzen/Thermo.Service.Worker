using System;
using System.Collections.Generic;

namespace Service.ThermoDataModel.Models
{
    public class AttendanceResponse
    {
        public int Command { get; set; }

        public AttendanceRecord[]  Data { get; set; }

        public string Setail { get; set; }

        public int RecordCount { get; set; }

        public int Status { get; set; }

        public int Transmit_Cast { get; set; }

    }

    public class AttendanceRecord
    {
        public string Guid { get; set; }

        public string Address { get; set; }
        
        public int Age { get; set; }
        
        public string Birth { get; set; }

        public string BodyTemperature { get; set; }

        public string CertificateNumber { get; set; }

        public int CertificateType { get; set; }

        public string Country { get; set; }

        public string DeviceId { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public string GroupId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Nation { get; set; }

        public string PersonId { get; set; }

        public string Phone { get; set; }

        public int Respirator { get; set; }

        public DateTime TimeStamp { get; set; }

        public string UserId { get; set; }

    }
}
