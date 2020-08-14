using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.EmailNotifier
{
    public interface IDataFilter
    {
        Task ExecuteDataFiltering(AttendanceRecord attendanceRecord, ILogger logger);
    }
}