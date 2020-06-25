using AzCloudApp.MessageProcessor.Core.PersonelThemoDataHandler;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Function
{
    public class PersonelTemperatureHandlingFunction
    {
        private readonly IMesssageThermoProcessor _messsageThermoProcessor;
        private readonly ILogger<PersonelTemperatureHandlingFunction> _logger;

        public PersonelTemperatureHandlingFunction(ILogger<PersonelTemperatureHandlingFunction> logger, 
            IMesssageThermoProcessor messsageThermoProcessor)
        {
            this._messsageThermoProcessor = messsageThermoProcessor;
            this._logger = logger;
        }

        [FunctionName("ThermoDataProcessorAzure")]
        public async Task Run([ServiceBusTrigger("%TargetQueueName%", 
            Connection = "sbqconnection")]string messageSource, ILogger log)
        {
            log.LogInformation("Hello there!!!!!! ");
            this._logger.LogInformation($"ThermoDataProcessorAzure started : {messageSource} {DateTime.Now}");
            await this._messsageThermoProcessor.ProcessMessage(messageSource);
        }
    }
}
