namespace Thermo.Web.WebApi.Model.SMTPModel
{
    public class SmtpGetResponse
    {
        public long Nid { get; set; }

        public int Company { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string HostName { get; set; }

        public string FromEmail { get; set; }

        public int Port { get; set; }

    }
}
