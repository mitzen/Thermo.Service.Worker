using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using ThermoDataModel;

namespace ThermoProcessWorker.RestServices
{
    public class ThermoDataRequester
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

        public async Task GetPersonelThermoDataAsync(IRestRequest request)
        {
            this._logger.LogInformation("Initiating request to obtain ThermoData");
             await this._client.PostAsync<PersonelThermoResponse>(request, this._token);
        }
    }
}