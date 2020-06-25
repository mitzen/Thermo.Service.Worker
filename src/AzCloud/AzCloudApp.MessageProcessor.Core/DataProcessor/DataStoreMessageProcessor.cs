using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Utils;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class DataStoreMessageProcessor : IDataStoreProcesor
    {
        private readonly ThermoDataContext _thermoDataContext;
        
        public DataStoreMessageProcessor(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        public Task<int> SavePersonAsync(string source)
        {
            var target = MessageConverter.GetMessageType<PersonImgDataMessageQueue>(source);
//            this._thermoDataContext.PersonImgs.Add(target.ToModel());
            return this._thermoDataContext.SaveChangesAsync();
        }

        public Task<int> SaveDevicesAsync(string source)
        {
            var target = MessageConverter.GetMessageType<DeviceDataMessageQueue>(source);
 //         this._thermoDataContext.Devices.Add(target.ToModel());
            return this._thermoDataContext.SaveChangesAsync();
        }

        public Task<int> SaveAttendRecordAsync(string source)
        {
//            var target = MessageConverter.GetMessageType<AttendRecordDataMessageQueue>(source);
//            this._thermoDataContext.AttendRecords.Add(target.ToModel());
            return this._thermoDataContext.SaveChangesAsync();
        }

        public Task<int> SavePersonImgAsync(string source)
        {
            var target = MessageConverter.GetMessageType<PersonImgDataMessageQueue>(source);
//            this._thermoDataContext.PersonImgs.Add(target.ToModel());
            return _thermoDataContext.SaveChangesAsync();
        }
    }
}
