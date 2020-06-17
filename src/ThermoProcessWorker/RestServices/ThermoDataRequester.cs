using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using ThermoDataModel;

namespace ThermoProcessWorker.RestServices
{
    public class ThermoDataRequester
    {
        private readonly IRestClient _client; 
        private CancellationToken _token;
        public ThermoDataRequester(IRestClient client, CancellationToken token)
        {
            this._client = client; 
            this._token = token;
        }

        public async Task GetPersonelThermoDataAsync(IRestRequest request)
        {
             await this._client.PostAsync<PersonelThermoResponse>(request, this._token);
        }
    }
}