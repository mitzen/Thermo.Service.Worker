using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface INotificationProcessor
    {
        Task<int> ProcessAsync(string source);
    }
}
