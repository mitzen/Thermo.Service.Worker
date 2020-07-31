using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Thermo.Web.WebApi.Model.UserModel;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util
{
    public static class UserConverter
    {
        public static void UpateModel(ref UsersDataStore source, NewUserRequest target)
        {
            source.FirstName = target.FirstName;
            source.LastName = target.LastName;
            source.NotificationToken = target.NotificationToken;
            source.Password = target.Password;
        }

        public static UsersDataStore ToModel(this NewUserRequest target)
        {
            var source = new UsersDataStore();
            source.FirstName = target.FirstName;
            source.LastName = target.LastName;
            source.NotificationToken = target.NotificationToken;
            source.Password = target.Password;
            return source;
        }
    }
}
