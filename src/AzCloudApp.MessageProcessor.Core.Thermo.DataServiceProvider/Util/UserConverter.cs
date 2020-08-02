using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Thermo.Web.WebApi.Model.UserModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util
{
    public static class UserConverter
    {
        public static void UpateModel(ref UsersDataStore target, UserNewRequest source)
        {
            target.CompanyId = source.CompanyId;
            target.Email = source.Email;
            target.FirebaseToken = source.FirebaseToken;
            target.Nid = source.Nid;
            target.Password = source.Password;
            target.Username = source.Username;
        }

        public static void UpateModel(ref UsersDataStore target, UserUpdateRequest source)
        {
            target.CompanyId = source.CompanyId;
            target.Email = source.Email;
            target.FirebaseToken = source.FirebaseToken;
            target.Nid = source.Nid;
            target.Password = source.Password;
            target.Username = source.Username;
        }

        public static UsersDataStore ToModel(this UserNewRequest target)
        {
            var source = new UsersDataStore();
            source.CompanyId = target.CompanyId;
            source.Email = target.Email;
            source.FirebaseToken = target.FirebaseToken;
            source.Password = target.Password;
            source.Username = target.Username;
            return source;
        }

        public static UsersDataStore ToModel(this UserUpdateRequest target)
        {
            var source = new UsersDataStore();
            source.CompanyId = target.CompanyId;
            source.Email = target.Email;
            source.FirebaseToken = target.FirebaseToken;
            source.Nid = target.Nid;
            source.Password = target.Password;
            source.Username = target.Username;
            return source;
        }
     
    }
}
