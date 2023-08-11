﻿using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Service.ThermoDataModel.Heartbeat;
using Service.ThermoDataModel.Models;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.DataProcessor
{
    public interface IDataStoreProcesor
    {        
        Task<int> SaveAttendanceRecordAsync(AttendanceRecord source);

        AttendanceDataStore GetAttendanceRecord(AttendanceRecord source);

        Task<int> SaveHeartBeatRecordAsync(HeartbeatMessage source);

        HeartBeatDataStore GetHeartBeateRecordByDeviceId(HeartbeatMessage source);
    }
}
