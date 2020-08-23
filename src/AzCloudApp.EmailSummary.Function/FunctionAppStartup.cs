using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AzCloudApp.MessageProcessor.Core.DataProcessor;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Service.MessageBusServiceProvider.Queue;
using AzCloudApp.MessageProcessor.Core.EmailSummary;

[assembly: FunctionsStartup(typeof(AzCloudApp.MessageProcessor.Function.FunctionAppStartup))]
namespace AzCloudApp.MessageProcessor.Function
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
             .AddOptions<EmailSummaryConfiguration>()
             .Configure<IConfiguration>((messageResponderSettings, configuration) =>
             {
                 configuration
                 .GetSection("EmailSummary")
                 .Bind(messageResponderSettings);
             });

            builder
            .Services
            .AddOptions<ServiceBusConfiguration>()
            .Configure<IConfiguration>((messageResponderSettings, configuration) =>
            {
                configuration
                .GetSection("NotificationServiceBus")
                .Bind(messageResponderSettings);
            });

            builder.Services.AddLogging();

            builder.Services.AddTransient<ISummaryEmailProviderDataProcessor, SummaryEmailProviderDataProcessor>();
            builder.Services.AddTransient<ISummaryServiceHandler, EmailSummaryServiceHandler>();
            builder.Services.AddTransient<ISummaryMailContentParser, SummaryMailContentParser>();

            builder.Services.AddDbContext<ThermoDataContext>(opt => opt.UseSqlServer(configBuilder.GetConnectionString("ThermoDatabase")));
        }
    }
}
