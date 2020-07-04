using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.AzBlob
{
    public interface IBlobClientProvider
    {
        Task PushImageToStoreAsync(string targetContainer, string path);
    }
}