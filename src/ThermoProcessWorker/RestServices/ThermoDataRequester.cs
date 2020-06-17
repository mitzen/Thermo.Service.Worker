using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using ThermoDataModel;

namespace ThermoProcessWorker.RestServices
{
    public class ThermoDataRequester : IThermoDataRequester
    {
        private readonly IRestClient _client; 
        private CancellationToken _token;
        
        private readonly ILogger _logger; 
        public ThermoDataRequester(IRestClient client, CancellationToken token, ILogger logger)
        {
            this._client = client; 
            this._token = token;
            _logger = logger;
        }

        public async Task<IRestResponse<T>> GetPersonelThermoDataAsync<T>(IRestRequest request) 
        where T : class
        {
            this._logger.LogInformation("Initiating request to obtain ThermoData");
            return await this._client.ExecuteAsync<T>(request);
        }
    }
}