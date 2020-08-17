using System.Collections.Generic;

namespace Service.ThermoDataModel.Models
{
    public class MailContentData
    {
        public MailInfo MailInfo { get; set; }
    }

    public class MailInfo
    {
        public IEnumerable<string> Recipients { get; set; }

        public string Subject { get; set; }

        public string ContentBody { get; set; }
        
        public string Sender { get; set; }

        public string SenderName { get; set; }
    }
}
