using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.PersonelThemoDataHandler
{
    public interface IMesssageThermoProcessor
    {
        Task ProcessMessage(string message, ILogger logger);
    }
}