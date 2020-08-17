namespace AzCloudApp.MessageProcessor.Core.EmailNotifier
{
    public class TemperatureFilterConfiguration
    {
        public double Max { get; set; }

        public string Subject { get; set; }

        public string EmailTemplateMaxTemperatureHit { get; set; }

        public string Sender { get; set; }

        public string SenderName { get; set; }

    }
}
