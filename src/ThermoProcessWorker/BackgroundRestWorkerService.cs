using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.MessageBusServiceProvider.CheckPointing;
using Service.ThermoDataModel.Configuration;
using Service.ThermoProcessWorker.AppBusinessLogic;

namespace Service.ThermoProcessWorker
{
    public class BackgroundRestWorkerService : BackgroundService
    {
        private const string ServiceWorkerConfiguration = "ServiceWorkerConfiguration";
        private readonly ILogger<BackgroundRestWorkerService> _logger;
        private readonly IConfiguration _configuration;
        private ServiceWorkerConfiguration _serviceWorkerConfiguration;
        private readonly ICheckPointLogger _checkPointLogger;
        private readonly IThermoDataLogic thermoLogic;

        public BackgroundRestWorkerService(ILogger<BackgroundRestWorkerService> logger, 
            IThermoDataLogic logic, 
            IConfiguration configuration, 
            ICheckPointLogger checkPointLogger)
        {
            _logger = logger;
            _configuration = configuration;
            var svc = configuration.GetSection(ServiceWorkerConfiguration);
                
            _serviceWorkerConfiguration = svc.Get<ServiceWorkerConfiguration>();
            _checkPointLogger = checkPointLogger;
            thermoLogic = logic;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"-----------------------------------------------------");
            _logger.LogInformation($"Service : Startup {DateTime.Now}");
            _logger.LogInformation($"-----------------------------------------------------");

            //var thermoLogic = new ThermoDataLogic(this._logger, this._configuration, _checkPointLogger);
            _serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond ??= 5000;
            ////////////////////////////////////////////////////////////////////
            thermoLogic.Setup(stoppingToken);
            ////////////////////////////////////////////////////////////////////

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"**************************************************");
                _logger.LogInformation($"Service : Executing job at {DateTime.Now}.");
                _logger.LogInformation($"**************************************************");
                ////////////////////////////////////////////////////////////////////
                await thermoLogic.ExecuteAsync();
                ////////////////////////////////////////////////////////////////////
                ///
                _logger.LogInformation($"**************************************************");
                _logger.LogInformation($"Service : completed {DateTime.Now}.");
                _logger.LogInformation($"**************************************************");
                _logger.LogInformation($"Service : Waiting to run again in {_serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond.Value} seconds. {DateTime.Now}.");
                
                await Task.Delay(new TimeSpan(0, 0, _serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond.Value),
                    stoppingToken);
            }

            _logger.LogInformation($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            _logger.LogInformation($"Service stopped or cancelled! {DateTime.Now}.");
            _logger.LogInformation($"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
    }    
}
