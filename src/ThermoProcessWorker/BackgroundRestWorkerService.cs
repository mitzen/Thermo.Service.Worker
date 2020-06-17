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
    public class BackgroundRestWorkerService : BackgroundService
    {
        private const string ServiceBusConfigurationKey = "ServiceBusConfiguration";
        private const string ThermoRestApiConfigurationKey = "ThermoConfiguration";
        private readonly ILogger<BackgroundRestWorkerService> _logger;
        private readonly IConfiguration _configuration;
        private readonly ThermoConfiguration _restConfiguration;
        private readonly ServiceBusConfiguration _serviceBusConfiguration;

        public BackgroundRestWorkerService(ILogger<BackgroundRestWorkerService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceBusConfiguration = configuration.GetSection(ServiceBusConfigurationKey).Get<ServiceBusConfiguration>();
            _restConfiguration = configuration.GetSection(ThermoRestApiConfigurationKey).Get<ThermoConfiguration>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("--------------------------------------------------------");
            _logger.LogInformation(_serviceBusConfiguration.QueueName);
            _logger.LogInformation(_serviceBusConfiguration.ServiceBusConnection);
            _logger.LogInformation(_restConfiguration.Hostname);
            _logger.LogInformation(_restConfiguration.PersonelUrl);
            _logger.LogInformation("--------------------------------------------------------");

            var targetBaseUrl = _restConfiguration.Hostname + _restConfiguration.PersonelUrl;
            var client = new RestClient(targetBaseUrl);
            var thermoDataRequester = new ThermoDataRequester(client, stoppingToken, this._logger);
            
            var request = new RestRequest();
            request.AddJsonBody(new RestRequest());

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await thermoDataRequester.GetPersonelThermoDataAsync(request);
                 
                // Run task 
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
