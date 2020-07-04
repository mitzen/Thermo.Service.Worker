using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.MessageBusServiceProvider.CheckPointing;
using Service.MessageBusServiceProvider.Queue;
using Service.ThermoProcessWorker.AppBusinessLogic;

namespace Service.ThermoProcessWorker
{
    public class Program
    {
        public IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();
                services.AddSingleton<IChannelMessageSender, ChannelMessageSender>();
                services.AddSingleton<ICheckPointLogger, CheckPointLogger>();
                services.AddSingleton<IThermoDataLogic, ThermoDataLogic>();
                services.AddHostedService<BackgroundRestWorkerService>();
            });
    }
}
