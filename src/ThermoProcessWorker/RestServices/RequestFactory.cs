using System.Threading;
using Microsoft.Extensions.Logging;
using RestSharp;
using Service.ThermoDataModel.Models.Test;

namespace Service.ThermoProcessWorker.RestServices
{
    public class RequestFactory
    {
        public static IThermoDataRequester CreateRestService(string targetBaseUrl, CancellationToken stoppingToken,
        ILogger logger)
        {
            var dataservice = new RestDataService(new RestClient(targetBaseUrl), stoppingToken, logger);
            return new ThermoDataRequester(dataservice, logger);
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