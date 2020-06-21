using System.Threading.Tasks;
using RestSharp;

namespace Service.ThermoProcessWorker.RestServices
{
    public interface IRestDataService
    {
        Task<IRestResponse<T>> ExecuteAsync<T>(IRestRequest request) 
        where T : class;
    }
}