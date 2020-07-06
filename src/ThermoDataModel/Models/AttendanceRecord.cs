using System;

namespace Service.ThermoDataModel.Models
{
    public class AttendanceResponse 
    {
        public int Command { get; set; }

        public string BatchId { get; set; }

        public AttendanceRecord[]  Data { get; set; }

        public string Detail { get; set; }

        public int RecordCount { get; set; }

        public int Status { get; set; }

        public int Transmit_Cast { get; set; }

    }

    public class AttendanceRecord : CoreMessage
    {
        public string Guid { get; set; }

        public string BatchId { get; set; }

        public string Subject { get; set; }

        public string Address { get; set; }
        
        public int Age { get; set; }
        
        public DateTime? Birth { get; set; }

        public double BodyTemperature { get; set; }

        public string CertificateNumber { get; set; }

        public int CertificateType { get; set; }

        public string Country { get; set; }

        public string DeviceId { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public string GroupId { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Nation { get; set; }

        public string PersonId { get; set; }

        public string Phone { get; set; }

        public int Respirator { get; set; }

        public DateTime? TimeStamp { get; set; }

        public string UserId { get; set; }

        public string Img { get; set; }

    }
}
