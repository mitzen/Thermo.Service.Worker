namespace Service.ThermoDataModel.Requests
{
    public class AttendanceRequest
    {

        public int StartId { get; set; }

        public int ReqCount { get; set; }

        public bool NeedImg { get; set; }
    }
}
