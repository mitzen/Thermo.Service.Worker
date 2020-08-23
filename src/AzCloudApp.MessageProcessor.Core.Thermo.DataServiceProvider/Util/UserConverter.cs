using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
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
            target.Password = source.Password;
            target.Username = source.Username;
            target.Role = source.Role;
        }

        public static void UpateModel(ref UsersDataStore target, UserUpdateRequest source)
        {
            target.CompanyId = source.CompanyId;
            target.Email = source.Email;
            target.FirebaseToken = source.FirebaseToken;
            target.Password = EncryptionUtil.Encrypt(source.Password);
            target.Username = source.Username;
            target.Role = source.Role;
        }

        public static UsersDataStore ToModel(this UserNewRequest source)
        {
            var target = new UsersDataStore();
            target.CompanyId = source.CompanyId;
            target.Email = source.Email;
            target.FirebaseToken = source.FirebaseToken;
            target.Password = EncryptionUtil.Encrypt(source.Password);
            target.Username = source.Username;
            target.Role = source.Role;
            return target;
        }

        public static UsersDataStore ToModel(this UserUpdateRequest target)
        {
            var source = new UsersDataStore();
            source.CompanyId = target.CompanyId;
            source.Email = target.Email;
            source.FirebaseToken = target.FirebaseToken;
            source.Nid = target.Nid;
            source.Password = EncryptionUtil.Encrypt(target.Password);
            source.Username = target.Username;
            return source;
        }
     
    }
}
