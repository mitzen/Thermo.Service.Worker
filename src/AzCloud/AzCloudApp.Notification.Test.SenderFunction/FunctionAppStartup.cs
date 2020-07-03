using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Service.ThermoDataModel.Configuration;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(AzCloudApp.Notification.Test.SenderFunction.FunctionAppStartup))]

namespace AzCloudApp.Notification.Test.SenderFunction
{
    public class FunctionAppStartup : FunctionsStartup
    {
        public FunctionAppStartup()
        {
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configBuilder = new ConfigurationBuilder()
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            builder
              .Services
              .AddOptions<NotificationConfiguration>()
              .Configure<IConfiguration>((messageResponderSettings, configuration) =>
              {
                  configuration
                  .GetSection("Notification")
                  .Bind(messageResponderSettings);
              });

            builder.Services.AddLogging();
            builder.Services.AddTransient<ISendMailService, SendMailService>();
            builder.Services.AddTransient<INotificationProcessor, NotificationMessageProcessor>();

            builder.Services.AddDbContext<ThermoDataContext>(opt => opt.UseSqlServer(configBuilder.GetConnectionString("ThermoDatabase")));

        }
    }
}
