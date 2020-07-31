namespace Thermo.Web.WebApi.Model.SMTPModel
{
    public class SMTPListRequest
    {
        public string Filter { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
