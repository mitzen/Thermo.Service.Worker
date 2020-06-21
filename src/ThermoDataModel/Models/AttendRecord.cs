using System;

namespace Service.ThermoDataModel.Models
{
    public class AttendRecord
    {
        public int AttendRecordId { get; set; }
        public int Id { get; set; }
        public string BodyTemperature { get; set; }
        public string Deviceid { get; set; }
        public string PersonId { get; set; }
        public int? Respirator { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Userid { get; set; }
        public string ImgBase64 { get; set; }
    }
}
