using System.Threading.Tasks;
using Thermo.Web.WebApi.Model.UserModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider
{
    public interface IPersonDataService
    {
        Task<int> SaveUserAsync(UserNewRequest source);
    }
}