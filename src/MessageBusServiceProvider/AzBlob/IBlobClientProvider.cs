using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.AzBlob
{
    public interface IBlobClientProvider
    {
        Task<string> PushImageToStoreAsync(string targetContainer, string path);
    }
}