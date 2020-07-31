using System;
using System.Collections.Generic;
using System.Text;

namespace Thermo.Web.WebApi.Model.SMTPModel
{
    public class NewSMTPRequest
    {
        public long Nid { get; set; }

        public string Name { get; set; }

        public string Host { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

    }
}
