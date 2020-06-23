namespace Service.ThermoDataModel.Configuration
{
    public class ServiceWorkerConfiguration
    {
        public int? GetDataFromRestServiceIntervalSecond { get; set; }

        public string LogFilePath { get; set; }

        public string CheckPointFilePath { get; set; }

    }
}
