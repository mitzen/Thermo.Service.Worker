using System;
using System.Collections.Generic;
using System.Linq;

namespace AzCloudApp.MessageProcessor.Core.Thermo.DataServiceProvider.Util
{
    public class DataTypeHelper
    {
        public static IEnumerable<int> ConvertToIntegerArray(string target)
        {
            return target.Split(',').Select(n => Convert.ToInt32(n)).ToList();
        }
    }
}
