namespace Service.ThermoDataModel.Models
{
    public class ThermoMailContent
    {
        public ThermoMailContent(MailingInfo mailingInfo, AttendanceRecord attendance)
        {
            this.AttendanceData = attendance;
            this.MailingInfoData = mailingInfo;
        }

        public MailingInfo MailingInfoData { get; set; }
        public AttendanceRecord AttendanceData { get; set; }
    }
}
