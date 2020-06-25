using Newtonsoft.Json;

namespace AzCloudApp.MessageProcessor.Core.MessageSenderFunction
{
    public class MessageConverter
    {
        public static string Serialize<T>(T sourceObject)
        {
            return JsonConvert.SerializeObject(sourceObject);
        }
    }
}