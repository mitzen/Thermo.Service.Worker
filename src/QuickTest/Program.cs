using Service.MessageBusServiceProvider.AzBlob;
using Service.MessageBusServiceProvider.Imaging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace QuickTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var tpath = @"c:\temp\test1\tes2\test3\test.tmp";
            var x = Path.GetDirectoryName(tpath);
            
            if (!Directory.Exists(x))
            {
               var d =  Directory.CreateDirectory(x);
            }
            //StoreImageToAzureStorage();

        }

        private static async Task StoreImageToAzureStorage()
        {
            try
            {
                var blob = new BlobClientProvider(null, null);
                await blob.PushImageToStoreAsync("attendance", @"c:\temp\1.png");
            }
            catch (Exception ex)
            {
                var x = ex.Message;
                throw;
            }
        }
    }
}
