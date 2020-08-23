using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System.Collections.Generic;
using Thermo.Web.WebApi.Model.UserModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Extensions
{
    public static class UserResponseMapper
    {
        public static UserGetResponse MapTo(this UsersDataStore source)
        {
            if (source != null)
            {
                var target = new UserGetResponse();
                target.CompanyId = source.CompanyId;
                target.Email = source?.Email.Trim();
                target.FirebaseToken = source?.FirebaseToken;
                target.Nid = source.Nid;
                target.Password = source.Password.Trim();
                target.TimeStamp = source.TimeStamp;
                target.Username = source.Username.Trim();
                target.Role = source.Role;

                return target;
            }
            return null;
        }

        public static IEnumerable<UserGetResponse> MapTo(this IEnumerable<UsersDataStore> source)
        {
            var list = new List<UserGetResponse>();

            foreach (var item in source)
            {
                list.Add(item.MapTo());
            }

            return list;
        } 
    }
}
