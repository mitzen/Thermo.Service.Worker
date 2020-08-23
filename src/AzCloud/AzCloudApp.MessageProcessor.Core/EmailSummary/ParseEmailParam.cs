using System.Collections.Generic;

namespace AzCloudApp.MessageProcessor.Core.EmailSummary
{
    public class ParseEmailParam
    {
        public int CompanyId { get; set; }

        public int TotalScans { get; set; }

        public int TotalAbnormalDetected { get; set; }

        public IEnumerable<string> Recipients { get; set; }

        public ParseEmailParam(int companyId, int emailCount)
        {
            CompanyId = companyId;
            TotalScans = emailCount;
        }
    }
}
