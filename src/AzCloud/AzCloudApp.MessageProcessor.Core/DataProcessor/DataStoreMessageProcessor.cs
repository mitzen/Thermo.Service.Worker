using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Utils;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Service.ThermoDataModel.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public class DataStoreMessageProcessor : IDataStoreProcesor
    {
        private readonly ThermoDataContext _thermoDataContext;
        
        public DataStoreMessageProcessor(ThermoDataContext thermoDataContext)
        {
            _thermoDataContext = thermoDataContext;
        }

        #region Commented methods

        //public Task<int> SavePersonImgAsync(string source)
        //{
        //    var target = MessageConverter.GetMessageType<PersonImgDataMessageQueue>(source);
        //    //            this._thermoDataContext.PersonImgs.Add(target.ToModel());
        //    return _thermoDataContext.SaveChangesAsync();
        //}

        //public Task<int> SavePersonAsync(string source)
        //{
        //    var target = MessageConverter.GetMessageType<PersonImgDataMessageQueue>(source);
        //    return this._thermoDataContext.SaveChangesAsync();
        //}

        //public Task<int> SaveDevicesAsync(string source)
        //{
        //    var target = MessageConverter.GetMessageType<DeviceDataMessageQueue>(source);
        //    this._thermoDataContext.Devices.Add(target.ToModel());
        //    return this._thermoDataContext.SaveChangesAsync();
        //}
        
        #endregion

        public Task<int> SaveAttendanceRecordAsync(AttendanceRecord source)
        {  
            this._thermoDataContext.AttendanceRecord.Add(source.ToModel());
            return this._thermoDataContext.SaveChangesAsync();
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
