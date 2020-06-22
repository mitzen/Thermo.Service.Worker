using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Configuration;
using Service.ThermoProcessWorker.AppBusinessLogic;

public class TimedBasedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedBasedService> _logger;
    private Timer _timer;
    private readonly IConfiguration _configuration;
    private IThermoDataLogic _thermoLogic;
    private ServiceWorkerConfiguration _serviceWorkerConfiguration;

    public TimedBasedService(ILogger<TimedBasedService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _serviceWorkerConfiguration = configuration.GetSection("ServiceWorkerConfiguration").Get<ServiceWorkerConfiguration>();
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Startup Service.");

        _thermoLogic = new ThermoDataLogic(this._logger, this._configuration, stoppingToken);
        //_thermoLogic.Setup();
        _timer = new Timer(GetThermoDataRestService1, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(_serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond ??= 10));

        _logger.LogInformation($"Pausing timer.{Thread.CurrentThread}");
        _timer.Change(5, 5000);

        return Task.CompletedTask;
    }

    private async void GetThermoDataRestService1(object state)
    {
        _logger.LogInformation($"Start running job");
        
        await Task.Delay(2000);
        
        _logger.LogInformation($"Timer : Updat to run in {_serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond.Value} seconds");
        _timer.Change(0, _serviceWorkerConfiguration.GetDataFromRestServiceIntervalSecond.Value);
    }

    private async void GetThermoDataRestService(object state)
    {
        try
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            var count = Interlocked.Increment(ref executionCount);
            await this._thermoLogic.ExecuteAsync();
            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}