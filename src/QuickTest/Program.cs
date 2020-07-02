using Service.MessageBusServiceProvider.AzBlob;
using System;
using System.Threading.Tasks;

namespace QuickTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var blob = new BlobClientProvider();

            await blob.PushImageToStore("?sv=2019-10-10&ss=bfqt&srt=sco&sp=rwdlacupx&se=2020-07-02T23:35:49Z&st=2020-07-02T15:35:49Z&spr=https,http&sig=EcF10jovl4gOnFy5F2mILVq4FVytnc4bOtFMzLbNOX4%3D", @"c:\temp\1.png");

        }
    }
}
