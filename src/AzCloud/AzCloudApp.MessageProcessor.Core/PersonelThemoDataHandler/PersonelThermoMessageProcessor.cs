using AzCloudApp.MessageProcessor.Core.MessageController;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.PersonelThemoDataHandler
{
    public class PersonelThermoMessageProcessor : IMesssageThermoProcessor
    {
        private readonly ILogger<PersonelThermoMessageProcessor> _logger;
        private readonly IMessageController _messageController;

        public PersonelThermoMessageProcessor(ILogger<PersonelThermoMessageProcessor> logger,
          IMessageController messageController)
        {
            _logger = logger;
            _messageController = messageController;
        }

        public async Task ProcessMessage(string message, ILogger logger)
        {
            logger.LogInformation($"Passing mesages received from service bus to PersonelThermoMessageProcessor : {DateTime.Now} : {message}");
            await _messageController.ProcessDataAsync(message, logger);
        }
    }
}
