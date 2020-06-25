namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class ThermoBaseMessageType
    {
        public int MessageType { get; set; }
    }

    public enum TaskStatus
    {
        Ok, 
        Error, 
        Partial 
    }
}