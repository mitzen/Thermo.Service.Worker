using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thermo.Web.WebApi.Util
{
    public class ClaimUtil
    {
        public static DateTime GetExpiryClaimExpiryDate(string date)
        { 
            if (double.TryParse(date, out double linuxTime))
            {
                return UnixTimeStampToDateTime(linuxTime);
            }
            return DateTime.MinValue;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
