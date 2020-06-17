using System;
using System.Threading;
using System.Threading.Tasks;
using MessageBusServiceProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ThermoProcessWorker
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly ILogger<BackgroundWorkerService> _logger;
        private readonly IConfiguration _configuration;

        public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = this._configuration.GetSection("ServiceBusConfiguration").Get<ServiceBusConfiguration>();
            
            _logger.LogInformation("--------------------------------------------------------");
            _logger.LogInformation(config.QueueName);
            _logger.LogInformation(config.ServiceBusConnection);
            _logger.LogInformation("--------------------------------------------------------");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
