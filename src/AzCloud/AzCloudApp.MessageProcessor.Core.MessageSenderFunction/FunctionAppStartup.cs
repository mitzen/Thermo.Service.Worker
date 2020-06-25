using AzCloudApp.MessageProcessor.Core.DataProcessor;
using AzCloudApp.MessageProcessor.Core.MessageController;
using AzCloudApp.MessageProcessor.Core.PersonelThemoDataHandler;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.ThermoDataModel.Configuration;

[assembly: FunctionsStartup(
    typeof(AzCloudApp.MessageProcessor.Core.MessageSenderFunction.FunctionAppStartup))]

namespace AzCloudApp.MessageProcessor.Core.MessageSenderFunction
{
    public class FunctionAppStartup : FunctionsStartup
    {
        public FunctionAppStartup()
        {

        }
        
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
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
            builder.Services.AddTransient<IDataStoreProcesor, DataStoreMessageProcessor>();
            builder.Services.AddTransient<INotificationProcessor, NotificationMessageProcessor>();
            builder.Services.AddTransient<IMessageController, ThermoMessageController>();
            builder.Services.AddTransient<IMesssageThermoProcessor, PersonelThermoMessageProcessor>();
            builder.Services.AddDbContext<ThermoDataContext>(opt => opt.UseSqlServer(config.GetConnectionString("ThermoDatabase")));
        }
    }
}
