using System.Threading;
using System.Threading.Tasks;

namespace Service.ThermoProcessWorker.AppBusinessLogic
{
    public interface IThermoDataLogic
    {
        void Setup(CancellationToken token);
        Task ExecuteAsync();
    }
}