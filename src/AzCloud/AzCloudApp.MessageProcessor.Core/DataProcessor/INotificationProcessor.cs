using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface INotificationProcessor
    {
        Task ProcessAsync(string source, ILogger logger);
    }
}
