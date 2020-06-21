using System;
using System.Collections.Generic;

namespace Service.ThermoDataModel.Models
{
    public class AttendanceResponse
    {
        public int command { get; set; }

        public AttendanceRecord[]  Data { get; set; }

        public string detail { get; set; }

        public int recordCount { get; set; }

        public int status { get; set; }

        public int transmit_Cast { get; set; }

    }

    public class AttendanceRecord
    {
        public string Address { get; set; }
        public int Age { get; set; }

        //public int AttendRecordId { get; set; }
        //public int Id { get; set; }
        //public string BodyTemperature { get; set; }
        //public string Deviceid { get; set; }
        //public string PersonId { get; set; }
        //public int? Respirator { get; set; }
        //public DateTime? Timestamp { get; set; }
        //public string Userid { get; set; }
        //public string ImgBase64 { get; set; }
    }
}
