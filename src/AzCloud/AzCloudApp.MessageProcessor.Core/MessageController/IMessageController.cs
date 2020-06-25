using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.MessageController
{
    public interface IMessageController
    {
        Task ProcessDataAsync(string sourceData);
    }
}