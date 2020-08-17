using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.EmailSummary
{
    public interface ISummaryServiceHandler
    {
        Task ProcessMessage(ILogger logger);
    }
}
