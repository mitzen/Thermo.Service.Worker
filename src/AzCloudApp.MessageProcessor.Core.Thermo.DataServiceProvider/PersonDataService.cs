using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thermo.Web.WebApi.Model.UserModel;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider
{
    public class PersonDataService
    {
        private readonly ThermoDataContext _thermoDataContext;

        public PersonDataService(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        public virtual Task<int> SaveUserAsync(NewUserRequest source)
        {
            var targetRecord = GetSingleUserAsync(source.Nid);

            if (targetRecord == null)
            {
                this._thermoDataContext.Users.Add(source.ToModel());
            }
            else
            {
                UserConverter.UpateModel(ref targetRecord, source);
            }

            return this._thermoDataContext.SaveChangesAsync();
        }

        public virtual UsersDataStore GetSingleUserAsync(int? source)
        {
            if (source.HasValue)
            {
                return this._thermoDataContext.Users.Where(x => x.Nid == source.Value).FirstOrDefault();
            }

            return null;
        }

        public virtual IEnumerable<UsersDataStore> GetUsersAsync(int? source)
        {
            if (source.HasValue)
            {
                return this._thermoDataContext.Users.Where(x => x.Nid == source.Value);
            }

            return null;
        }

        public virtual Task<int> DeleteUserAsync(DeleteUserRequest source)
        {
            var targetRecord = GetSingleUserAsync(source.Nid);

            if (targetRecord == null)
            {
                this._thermoDataContext.Users.Remove(targetRecord);
            }

            return this._thermoDataContext.SaveChangesAsync();
        }
    }
}

