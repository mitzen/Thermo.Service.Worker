using System.Threading.Tasks;
using RestSharp;

namespace Service.ThermoProcessWorker.RestServices
{
   public interface IThermoDataRequester
    {
        Task<IRestResponse<T>> GetPersonelThermoDataAsync<T>(IRestRequest request) where T: class;
    }
}