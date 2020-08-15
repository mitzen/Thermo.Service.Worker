using Microsoft.Extensions.Logging;
using Service.MessageBusServiceProvider.Converters;
using Service.ThermoDataModel.Checkpoint;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.CheckPointing
{
    public class CheckPointLogger : ICheckPointLogger
    {
        private readonly ILogger<CheckPointLogger> _logger;
        public CheckPointLogger(ILogger<CheckPointLogger> logger)
        {
            _logger = logger;
        }

        public Task<bool> WriteCheckPoint(string fileName, CheckPointConfiguration checkPointConfiguration)
        {
            _logger.LogInformation($"Saving checkpointing to {fileName}, {checkPointConfiguration.LastSequence}");

            using (StreamWriter writetext = new StreamWriter(fileName, false))
            {
                writetext.WriteLine(MessageConverter.Serialize(checkPointConfiguration));
            }
            return Task.FromResult(false);
        }

        public Task<CheckPointConfiguration> ReadCheckPoint(string fileName)
        {
            string isourceContent = null;

            if (File.Exists(fileName))
            {
                using (StreamReader readtext = new StreamReader(fileName))
                {
                    isourceContent = readtext.ReadToEnd();
                }
            }
            else
            {
                return Task.FromResult(new CheckPointConfiguration
                {
                     LastSequence = 1, 
                });
            }

            var checkpointing = MessageConverter.DeSerialize<CheckPointConfiguration>(isourceContent);

            if (checkpointing != null)
                _logger.LogInformation($"Reading checkpoint to {fileName}, Last sequence count : {checkpointing.LastSequence}");
            else
                checkpointing = new CheckPointConfiguration { LastSequence = 1, LastUpdate = DateTime.Now };

            return Task.FromResult(checkpointing);
        }
    }

    public interface ICheckPointLogger
    {
        Task<bool> WriteCheckPoint(string fileName, CheckPointConfiguration checkPointConfiguration);

        Task<CheckPointConfiguration> ReadCheckPoint(string fileName);
    }
}