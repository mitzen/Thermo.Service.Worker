using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Utils;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Service.ThermoDataModel.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Service.ThermoDataModel.Heartbeat;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class DataStoreMessageProcessor : IDataStoreProcesor
    {
        private readonly ThermoDataContext _thermoDataContext;

        public DataStoreMessageProcessor(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        public Task<int> SaveAttendanceRecordAsync(AttendanceRecord source)
        {
            this._thermoDataContext.AttendanceRecord.Add(source.ToModel());
            return this._thermoDataContext.SaveChangesAsync();
        }

        public Task<int> SaveHeartBeatRecordAsync(HeartbeatMessage source)
        {
            var instance = GetHeartBeateRecordByDeviceId(source);

            if (instance != null)
            {
                instance.Timestamp = source.Timestamp;
            }
            else
            {
                this._thermoDataContext.HeartBeat.Update(source.ToModel());
            }

            return this._thermoDataContext.SaveChangesAsync();
        }

        public HeartBeatDataStore GetHeartBeateRecordByDeviceId(HeartbeatMessage source)
        {
            if (source != null && source.DeviceId != null)
            {
                var result = this._thermoDataContext.HeartBeat.Where(x => x.DeviceId.Trim().ToLower() == source.DeviceId.Trim().ToLower()).FirstOrDefault();

                return result;
            }
            return null;
        }

        public Task<List<AttendanceDataStore>> GetAttendanceRecordAsync(AttendanceRecord source)
        {
            if (!string.IsNullOrWhiteSpace(source.Email) ||
                !string.IsNullOrWhiteSpace(source.Name))
            {
                return this._thermoDataContext.AttendanceRecord.Where(x => x.Name.Trim().ToLower() ==
                source.Name.Trim().ToLower()
                && x.Email.Trim().ToLower() == source.Email.Trim().ToLower()).ToListAsync();
            }

            return Task.FromResult(new List<AttendanceDataStore>());
        }
    }
}
