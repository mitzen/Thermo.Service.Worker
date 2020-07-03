using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class AttendanceRecord
    {
        public string Guid { get; set; }

        //public string BatchId { get; set; }

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

        [Key]
        public long Nid { get; set; }

        public string Name { get; set; }

        public string Nation { get; set; }

        public string PersonId { get; set; }

        public string Phone { get; set; }

        public int Respirator { get; set; }

        public string TimeStamp { get; set; }

        public string UserId { get; set; }

    }
}
