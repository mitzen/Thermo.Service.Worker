using System;
using System.Collections.Generic;
using System.Text;

namespace Thermo.Web.WebApi.Model.MobileAPIModel
{
    public class UpdateFirebase
    {
        public string Username { get; set; }

        public string FirebaseToken { get; set; }

        public string UpdateType { get; set; }
    }
}
