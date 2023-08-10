using System;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class QueryTotalScanParam
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double TemperatureMax { get; set; }
    }
}
