using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Service.ThermoDataModel.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface IDataStoreProcesor
    {        
        Task<int> SaveAttendanceRecordAsync(AttendanceRecord source);

        Task<List<AttendanceDataStore>> GetAttendanceRecordAsync(AttendanceRecord source);
    }
}
