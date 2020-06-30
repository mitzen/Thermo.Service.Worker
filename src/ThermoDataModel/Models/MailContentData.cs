using System.Collections.Generic;

namespace Service.ThermoDataModel.Models
{
    public class MailContentData
    {
        public MailInfo MailInfo { get; set; }
        public AttendanceRecord AttendanceData { get; set; }
    }

    public class MailInfo
    {
        public string From { get; set; }

        public IEnumerable<string> Recipients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string IsBodyHtml { get; set; }
        public string Sender { get; set; }
    }
}
