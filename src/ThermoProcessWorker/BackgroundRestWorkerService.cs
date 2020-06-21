using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.AppBusinessLogic;

namespace Service.ThermoProcessWorker
{
    public class BackgroundRestWorkerService : BackgroundService
    {
        private readonly ILogger<BackgroundRestWorkerService> _logger;
        private readonly IConfiguration _configuration;

        public BackgroundRestWorkerService(ILogger<BackgroundRestWorkerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var thermoLogic = new ThermoDataLogic(this._logger, this._configuration, stoppingToken);
            thermoLogic.Setup();
       
            while (!stoppingToken.IsCancellationRequested)
            {
                await thermoLogic.ExecuteAsync();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }    
}
