namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class DeviceDataMessageQueue : ThermoBaseMessageType
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string IPAddress { get; set; }
        public bool? IsActive { get; set; }
    }
}