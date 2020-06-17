using System.Threading.Tasks;

namespace ThermoProcessWorker.AppBusinessLogic
{
    public interface IThermoDataLogic
    {
        void Setup();
        Task ExecuteAsync();
    }
}