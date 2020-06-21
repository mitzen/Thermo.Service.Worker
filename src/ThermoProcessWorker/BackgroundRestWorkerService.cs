using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Configuration;
using Service.ThermoProcessWorker.AppBusinessLogic;

namespace Service.ThermoProcessWorker
{
    public class BackgroundRestWorkerService : BackgroundService
    {
        private readonly ILogger<BackgroundRestWorkerService> _logger;
        private readonly IConfiguration _configuration;
        private ServiceWorkerConfiguration _serviceWorkerConfiguration;

        public BackgroundRestWorkerService(ILogger<BackgroundRestWorkerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceWorkerConfiguration = configuration.GetSection("ServiceWorkerConfiguration").Get<ServiceWorkerConfiguration>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Service : Startup {DateTime.Now}");
            var thermoLogic = new ThermoDataLogic(this._logger, this._configuration, stoppingToken);
            _serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond ??= 5000;
            ////////////////////////////////////////////////////////////////////
            //thermoLogic.Setup();
            ////////////////////////////////////////////////////////////////////

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Service : Executing job at {DateTime.Now}.");
                ////////////////////////////////////////////////////////////////////
                //await thermoLogic.ExecuteAsync();
                ////////////////////////////////////////////////////////////////////
                _logger.LogInformation($"Service : Reseting timer to run in" +
                    $"{_serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond.Value} seconds. {DateTime.Now}.");
                await Task.Delay(new TimeSpan(0, 0, _serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond.Value),
                    stoppingToken);
                _logger.LogInformation($"Service : completed {DateTime.Now}.");
            }
        }
    }    
}
