using System.Collections.Generic;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface IEmailAlertRecipientDataProcessor
    {
        IEnumerable<string> GetEmailByDeviceId(string deviceId);
    }
}
