using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ThermoProcessWorker.AppBusinessLogic;

public class TimedBasedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedBasedService> _logger;
    private Timer _timer;
    private readonly IConfiguration _configuration;
    private IThermoDataLogic _thermoLogic;

    public TimedBasedService(ILogger<TimedBasedService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _thermoLogic = new ThermoDataLogic(this._logger, this._configuration, stoppingToken);
        _thermoLogic.Setup();
       
        _timer = new Timer(GetThermoDataRestService, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private async void GetThermoDataRestService(object state)
    {
        var count = Interlocked.Increment(ref executionCount);

        await this._thermoLogic.ExecuteAsync();

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
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