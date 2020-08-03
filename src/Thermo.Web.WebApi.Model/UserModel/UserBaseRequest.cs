namespace Thermo.Web.WebApi.Model.UserModel
{
    public class UserBaseResponse
    {
        public int Nid { get; set; }

        public int Status { get; set; }

        public string Message { get; set; }
    }


    public class UserBaseRequest
    {
        public int Nid { get; set; }

        public string Role { get; set; }


        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int CompanyId { get; set; }

        public string FirebaseToken { get; set; }
    }

}
