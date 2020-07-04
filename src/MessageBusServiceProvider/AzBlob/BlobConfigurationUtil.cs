using Microsoft.Extensions.Configuration;
using Service.ThermoDataModel.Configuration;

namespace Service.MessageBusServiceProvider.AzBlob
{
    public class BlobConfigurationUtil
    {   
        public static BlobConfiguration GetBlobConfigiration(IConfiguration configuration)
        {
            return configuration.GetSection(BlobClientProvider.BlobConfigurationKey).Get<BlobConfiguration>();
        }
    }
}
