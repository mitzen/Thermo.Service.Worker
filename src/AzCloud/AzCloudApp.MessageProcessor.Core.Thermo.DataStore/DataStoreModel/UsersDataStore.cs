using System;
using System.ComponentModel.DataAnnotations;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel
{
    public class UsersDataStore
    {
        [Key]
        public int Nid { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int CompanyId { get; set; }

        public string FirebaseToken { get; set; }

        public string Role { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}
