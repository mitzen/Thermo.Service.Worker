using System;
using System.Threading;
using System.Threading.Tasks;
using MessageBusServiceProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;
using ThermoProcessWorker.RestServices;

namespace ThermoProcessWorker
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly ILogger<BackgroundWorkerService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ThermoConfiguration _restConfiguration;

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

            var targetBaseUrl = _restConfiguration.Hostname + _restConfiguration.PersonelUrl;
            var client = new RestClient(targetBaseUrl);
            var thermoDataRequester = new ThermoDataRequester(client);
            
            var request = new RestRequest();
            request.AddJsonBody(new RestRequest());

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await thermoDataRequester.GetPersonelThermoDataAsync(request, (status, handle) => 
                {
                    _logger.LogInformation(handle.WebRequest.Host);
                });
                 
                // Run task 
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
