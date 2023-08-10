using System;

namespace AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils
{
    public class EmailTemperatureHitParameter
    {
        public string DeviceId { get; set; }

        public string EmailMessage { get; set; }

        public string Location { get; set; }

        public double TemperatureRegistered { get; set; }

        public string Image { get; set; }

        public DateTime? Timestamp { get; set; }

        public EmailTemperatureHitParameter(string deviceId, string emailMessage)
        {
            DeviceId = deviceId;
            EmailMessage = emailMessage;
        }
    }
}
