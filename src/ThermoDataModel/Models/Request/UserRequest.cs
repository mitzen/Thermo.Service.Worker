using System;
using System.Collections.Generic;
using System.Text;

namespace Service.ThermoDataModel.Models.Request
{
    public class UserRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string NotificationToken { get; set; }

    }
}
