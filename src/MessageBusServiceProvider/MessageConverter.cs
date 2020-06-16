using Newtonsoft.Json;

namespace MessageBusServiceProvider
{
    public class MessageConverter
    {

        public static string Serialize<T>(T sourceObject)
        {
            return JsonConvert.SerializeObject(sourceObject);
        }

        public static T DeSerialize<T>(string sourceObject)
        {
            return (T) JsonConvert.DeserializeObject(sourceObject);
        }
    }
}