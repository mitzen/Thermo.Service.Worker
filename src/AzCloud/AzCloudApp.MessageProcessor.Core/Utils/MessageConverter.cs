using Newtonsoft.Json;

namespace AzCloudApp.MessageProcessor.Core.Utils
{
    public static class MessageConverter
    {
        public static T GetMessageType<T>(string sourceData)
        {
            return JsonConvert.DeserializeObject<T>(sourceData);
        }
        public static float GetFloatValue(this string sourceData)
        {
            return float.TryParse(sourceData, out float result) ? result : -1;
        }
    }
}
