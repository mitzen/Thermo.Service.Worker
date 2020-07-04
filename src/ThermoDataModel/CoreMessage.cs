namespace Service.ThermoDataModel
{
    public class CoreMessage
    {
        public string MessageType { get; set; }
    }

    public class CoreMessageType
    {
        public const string AttendanceMessage = "01";

        public const string HeartBeatMessage = "02";

    }
}