using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.AttendanceDataRuleFilter
{
    public interface IDataFilter
    {
        Task ExecuteDataFiltering(AttendanceRecord attendanceRecord, ILogger logger);
    }
}