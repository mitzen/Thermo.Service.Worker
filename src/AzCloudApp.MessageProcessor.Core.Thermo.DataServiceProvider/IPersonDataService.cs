using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider
{
    public interface IPersonDataService
    {
        UsersDataStore GetUserByName(string username);
    }
}