using Service.MessageBusServiceProvider.Converters;
using Service.ThermoDataModel.Checkpoint;
using System.IO;
using System.Threading.Tasks;

namespace Service.MessageBusServiceProvider.CheckPointing
{
    public class CheckPointLogger : ICheckPointLogger
    {  
        public Task<bool> WriteCheckPoint(string fileName, CheckPointConfiguration checkPointConfiguration)
        {
            using (StreamWriter writetext = new StreamWriter(fileName, false))
            {
                writetext.WriteLine(MessageConverter.Serialize(checkPointConfiguration));
            }

            return Task.FromResult(false);
        }

        public Task<string> ReadCheckPoint(string fileName)
        {
            string readText = null;
            using (StreamReader readtext = new StreamReader(fileName))
            {
                readText = readtext.ReadToEnd();
            }

            return Task.FromResult(readText);
        }
    }

    public interface ICheckPointLogger
    {
        Task<bool> WriteCheckPoint(string fileName, CheckPointConfiguration checkPointConfiguration);

        Task<string> ReadCheckPoint(string fileName);
    }
}