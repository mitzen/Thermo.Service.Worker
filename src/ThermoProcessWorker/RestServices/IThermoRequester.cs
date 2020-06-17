using System.Threading.Tasks;
using RestSharp;

namespace ThermoProcessWorker.RestServices
{
   public interface IThermoDataRequester
    {
        Task<IRestResponse<T>> GetPersonelThermoDataAsync<T>(IRestRequest request) where T: class;
    }
}