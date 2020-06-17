using System.Threading;
using Microsoft.Extensions.Logging;
using RestSharp;
using ThermoDataModel.Models.Test;

namespace ThermoProcessWorker.RestServices
{
    public class RequestFactory 
    {
        public static IThermoDataRequester CreateRestService(string targetBaseUrl, CancellationToken stoppingToken, 
        ILogger logger)
        {
            var client = new RestClient(targetBaseUrl);
            return new ThermoDataRequester(client, stoppingToken, logger);
        }

        public static RestRequest CreatePersonRequest(string url, Person source) 
        {
            var request = new RestRequest(url);
            request.Method = Method.POST;
            request.AddJsonBody(source);
            return request;
        }
    }
} 