using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;

namespace AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils
{
    public interface IMailContentParser
    {
        MailContentData CreateTemperatureEmailAlertMessage(EmailTemperatureHitParameter emaiInfoParameter, ILogger logger);
    }
}
