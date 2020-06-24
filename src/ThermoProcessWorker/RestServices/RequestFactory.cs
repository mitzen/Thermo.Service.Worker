using System.Threading;
using Microsoft.Extensions.Logging;
using RestSharp;
using Service.MessageBusServiceProvider.Converters;

namespace Service.ThermoProcessWorker.RestServices
{
    public class RequestFactory
    {
        public static IThermoDataRequester CreateRestService(string targetBaseUrl,
        ILogger logger)
        {
            var restClient = new RestClient(targetBaseUrl);
            var dataservice = new RestDataService(restClient, logger);
            return new ThermoDataRequester(dataservice, logger);
        }

        public static RestRequest CreatePostBodyRequest<T>(string url, T source)
        {
            var request = new RestRequest(url);
            request.Method = Method.POST;
            string src = MessageConverter.SerializeCamelCase(source);
            request.AddJsonBody(src);
            return request;
        }
    }
}