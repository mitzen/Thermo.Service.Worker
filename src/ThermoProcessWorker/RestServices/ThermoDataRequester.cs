using System;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;

namespace ThermoProcessWorker.RestServices
{
    public class ThermoDataRequester
    {
        IRestClient _client; 
        public ThermoDataRequester(IRestClient client)
        {
            this._client = client; 
        }

        public async Task GetPersonelThermoDataAsync(RestRequest request, 
        Action<IRestResponse,  RestRequestAsyncHandle> action)
        {
             this._client.PostAsync(request, action);
        }
        
    }
}