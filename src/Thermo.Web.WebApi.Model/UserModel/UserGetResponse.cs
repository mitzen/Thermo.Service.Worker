using System;

namespace Thermo.Web.WebApi.Model.UserModel
{
    public class UserGetResponse
    {
        public int Nid { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int? CompanyId { get; set; }

        public string FirebaseToken { get; set; }

        public DateTime? TimeStamp { get; set; }
    }
}
