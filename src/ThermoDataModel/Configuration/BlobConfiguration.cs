namespace Service.ThermoDataModel.Configuration
{
    public class BlobConfiguration
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }

        public string ImageStorePath { get; set; }
    }
}
