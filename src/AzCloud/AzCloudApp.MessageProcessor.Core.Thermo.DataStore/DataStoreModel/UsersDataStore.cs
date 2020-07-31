using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class UsersDataStore
    {
        [Key]
        public long Nid { get; set; }

        public string Guid { get; set; }

        public long Id { get; set; }

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

        public string Name { get; set; }

        public string Nation { get; set; }

        public string PersonId { get; set; }

        public string Phone { get; set; }

        public int Respirator { get; set; }

        public DateTime? TimeStamp { get; set; }

        public string UserId { get; set; }

        public string ImageUri { get; set; }

    }
}
