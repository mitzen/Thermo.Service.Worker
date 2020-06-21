using System.Threading.Tasks;

namespace Service.ThermoProcessWorker.AppBusinessLogic
{
    public interface IThermoDataLogic
    {
        void Setup();
        Task ExecuteAsync();
    }
}