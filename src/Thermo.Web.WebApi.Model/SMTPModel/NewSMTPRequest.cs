using System;
using System.Collections.Generic;
using System.Text;

namespace Thermo.Web.WebApi.Model.SMTPModel
{
    public class NewSMTPRequest
    {
        public int Nid { get; set; }

        public string Name { get; set; }

        public string HostName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public int Company { get; set; }

        public string FromEmail { get; set; }


    }
}
