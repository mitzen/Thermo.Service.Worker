using System.Collections.Generic;

namespace Service.ThermoDataModel.Models
{
    public class MailingInfo
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string HtmlContent { get; set; }
        public IEnumerable<string> Recipients { get; set; }
        public string SenderAddress { get; set; }
    }
}