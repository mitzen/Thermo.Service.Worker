using System.ComponentModel.DataAnnotations;


namespace Thermo.Web.WebApi.Model
{
    public class AuthenticateModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
