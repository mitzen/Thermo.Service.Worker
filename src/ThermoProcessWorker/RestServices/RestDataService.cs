using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Service.ThermoProcessWorker.RestServices
{
    public class RestDataService : IRestDataService 
    {
        private readonly IRestClient _client; 
        private CancellationToken _token;
        
        private readonly ILogger _logger; 
        public RestDataService(IRestClient client, CancellationToken token, ILogger logger)
        {
            this._client = client; 
            this._token = token;
            this._logger = logger;
        }

          public async Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request) 
        where T : class
        {
            this._logger.LogInformation($"Initiating request to {_client.BaseUrl}{request.Resource}");
            return await this._client.ExecuteAsync<T>(request);
        }
    }
}