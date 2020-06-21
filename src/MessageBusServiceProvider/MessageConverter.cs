using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Service.MessageBusServiceProvider
{
    public class MessageConverter
    {
        public static string SerializeCamelCase(object sourceObject)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(sourceObject, serializerSettings);
        }

        public static string Serialize(object sourceObject)
        {
            return JsonConvert.SerializeObject(sourceObject);
        }

        public static T DeSerializeCamelCase<T>(string sourceObject)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.DeserializeObject<T>(sourceObject, serializerSettings);
        }

        public static T DeSerialize<T>(string sourceObject)
        {
            return JsonConvert.DeserializeObject<T>(sourceObject);
        }
    }
}