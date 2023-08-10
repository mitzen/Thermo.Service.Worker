using Moq.AutoMock;
using Service.ThermoDataModel.Models;

namespace Service.ThermoProcessWorker.UnitTests.AppBusinessLogic
{
    public class BaseTest
    {
        protected AutoMocker mocker = new AutoMocker();
        
        public AttendanceRecord GetAttendanceData(string messageType)
        {
            var attendanceData = new AttendanceRecord()
            {
                BatchId = "1",
                Email = "test@test.com",
                Address = "test address",
                Age = 12,
                DeviceId = "DEVICE_ID",
                MessageType = messageType
            };

            return attendanceData;
        }
    }
}