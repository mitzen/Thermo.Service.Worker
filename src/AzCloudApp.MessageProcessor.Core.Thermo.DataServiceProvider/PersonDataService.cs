using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thermo.Web.WebApi.Model.UserModel;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util;
using AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Extensions;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider
{
    public class PersonDataService
    {
        private readonly ThermoDataContext _thermoDataContext;

        public PersonDataService(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        public virtual Task<int> SaveUserAsync(UserUpdateRequest source)
        {
            var targetRecord = GetUser(source.Nid);

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

        public virtual Task<int> RegisterUserAsync(UserNewRequest source)
        {
            var targetRecord = GetUser(source.Nid);

            if (targetRecord == null)
            {
                this._thermoDataContext.Users.Add(source.ToModel());
            }
            else
            {
                return Task.FromResult(-1);
            }

            return this._thermoDataContext.SaveChangesAsync();
        }

        public virtual UserGetResponse GetUserByIdAsync(int? source)
        {
            if (source.HasValue)
            {
                var result = this._thermoDataContext.Users.Where(x => x.Nid == source.Value).FirstOrDefault();

                return result.MapTo();
            }

            return null;
        }

        public virtual IEnumerable<UserGetResponse> GetUsersAsync()
        {
            return this._thermoDataContext.Users.MapTo().ToList();
        }

        public virtual Task<int> DeleteUserAsync(UserDeleteRequest source)
        {
            // if (source  null)
            //     return -1;

            var usersToRemove = DataTypeHelper.ConvertToIntegerArray(source?.TargetUsers);

            foreach (var targetUserId in usersToRemove)
            {
                var targetRecord = GetUser(targetUserId);

                if (targetRecord == null)
                {
                    _thermoDataContext.Users.Remove(targetRecord);
                }
            }

            return _thermoDataContext.SaveChangesAsync();
        }

        private UsersDataStore GetUser(int? source)
        {
            if (source.HasValue)
            {
                return this._thermoDataContext.Users.Where(x => x.Nid == source.Value).FirstOrDefault();
            }

            return null;
        }
    }
}

