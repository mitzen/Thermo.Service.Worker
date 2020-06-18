using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace ThermoProcessWorker.RestServices
{
    public class ThermoDataRequester : IThermoDataRequester
    {
        private readonly IRestDataService _restDataService;
        private readonly ILogger _logger; 

        public ThermoDataRequester(IRestDataService restDataService, ILogger logger)
        {
            _restDataService = restDataService;
            _logger = logger;
        }

        public async Task<IRestResponse<T>> GetPersonelThermoDataAsync<T>(IRestRequest request) 
        where T : class
        {
            this._logger.LogInformation("Initiating GetPersonelThermoDataAsync request.");
            return await this._restDataService.ExecuteAsync<T>(request);
        }
    }
}