using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System;
using System.Collections.Generic;
using System.Text;
using Thermo.Web.WebApi.Model.UserModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Extensions
{
    public static class UserResponseMapper
    {
        public static UserGetResponse MapTo(this UsersDataStore source)
        {
            var target = new UserGetResponse();
            target.CompanyId = source.CompanyId;
            target.Email = source.Email;
            target.FirebaseToken = source.FirebaseToken;
            target.Nid = source.Nid;
            target.Password = source.Password;
            target.TimeStamp = source.TimeStamp;
            target.Username = source.Username;

            return target;
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
