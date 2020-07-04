using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Utils;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Service.ThermoDataModel.Models;
using System.Linq;
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
            var targetRecord = GetAttendanceRecordAsync(source);
            if (targetRecord ==  null)
            {
                this._thermoDataContext.AttendanceRecord.Add(source.ToModel());
            }
            else
            {
                DataStoreModelConverter.UpateModel(ref targetRecord, source);
            }

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

        public AttendanceDataStore GetAttendanceRecordAsync(AttendanceRecord source)
        {
            if (source != null)
            {
                return this._thermoDataContext.AttendanceRecord.Where(x => x.Id == source.Id).FirstOrDefault();
            }

            return null;
        }
    }
}
