using System;
using System.Collections.Generic;
using System.Text;

namespace Thermo.Web.WebApi.Model.MobileAPIModel
{
    public class QueryAttendanceRecordRequest
    {
        public int CompanyId { get; set; }

        public DateTime QueryDate { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int NextId { get; set; }

        public String PersonId { get; set; }

    }
}
