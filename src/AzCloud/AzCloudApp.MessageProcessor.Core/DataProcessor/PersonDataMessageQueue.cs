using Service.ThermoDataModel.Models;
using System;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class PersonImgDataMessageQueue : ThermoBaseMessageType 
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string ImgBase64 { get; set; }
    }


    public class DeviceDataMessageQueue : ThermoBaseMessageType
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string IPAddress { get; set; }
        public bool? IsActive { get; set; }
    }

    public class PersonDataMessageQueue : ThermoBaseMessageType
    {
        public int Id { get; set; }
        public string CertificateNumber { get; set; }
        public int? CertificateType { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string GroupId { get; set; }
        public string Name { get; set; }
        public string PersonId { get; set; }
        public string Phone { get; set; }
        public string Userid { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}