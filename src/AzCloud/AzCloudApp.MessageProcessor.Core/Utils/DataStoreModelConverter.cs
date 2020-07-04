using AzCloudApp.MessageProcessor.Core.DataProcessor;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using Service.ThermoDataModel.Heartbeat;
using Service.ThermoDataModel.Models;

namespace AzCloudApp.MessageProcessor.Core.Utils
{
    public static class DataStoreModelConverter
    {
        #region Commented methods

        //public static Person ToModel(this PersonDataMessageQueue source)
        //{
        //    return new Person
        //    {
        //        Id = source.Id,
        //        CertificateNumber = source.CertificateNumber,
        //        CertificateType = source.CertificateType,
        //        Email = source.Email,
        //        Gender = source.Gender,
        //        GroupId = source.GroupId,
        //        Name = source.Name,
        //        PersonId = source.PersonId,
        //        Phone = source.Phone,
        //        Userid = source.Userid,
        //        UpdateTime = source.UpdateTime
        //    };
        //}

        //public static Device ToModel(this DeviceDataMessageQueue source)
        //{
        //    return new Device
        //    {
        //        Id = source.Id,
        //        DeviceId = source.DeviceId,
        //        IPAddress = source.IPAddress,
        //        IsActive = source.IsActive
        //    };
        //}

        //public static PersonImg ToModel(this PersonImgDataMessageQueue source)
        //{
        //    return new PersonImg
        //    {
        //        Id = source.Id,
        //        PersonId = source.PersonId,
        //        ImgBase64 = source.ImgBase64
        //    };
        //} 
        #endregion

        public static HeartBeatDataStore ToModel(this HeartbeatMessage source)
        {
            return new HeartBeatDataStore
            {
                Status = source.Status,
                DeviceId = source.DeviceId, 
                Timestamp = source.Timestamp
            };
        }

        public static AttendanceDataStore ToModel(this AttendanceRecord source)
        {
            return new AttendanceDataStore
            {
                Address = source.Address,
                Age = source.Age,
                Birth = source.Birth,
                BodyTemperature = source.BodyTemperature,
                CertificateNumber = source.CertificateNumber,
                CertificateType = source.CertificateType, 
                Country = source.Country,
                DeviceId = source.DeviceId,
                Email = source.Email,
                Gender = source.Gender,
                GroupId = source.GroupId, 
                Guid = source.Guid, 
                Name = source.Name,
                Nation = source.Nation,
                PersonId = source.PersonId,
                Phone = source.Phone,
                Respirator = source.Respirator,
                TimeStamp = source.TimeStamp,
                UserId = source.UserId
            };
        }
    }
}
