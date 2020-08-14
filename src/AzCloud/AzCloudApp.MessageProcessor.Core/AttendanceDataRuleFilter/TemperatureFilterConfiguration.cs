namespace AzCloudApp.MessageProcessor.Core.AttendanceDataRuleFilter
{
    public class TemperatureFilterConfiguration
    {
        public double Max { get; set; }

        public string EmailTemplateMaxTemperatureHit { get; set; }
    }
}
