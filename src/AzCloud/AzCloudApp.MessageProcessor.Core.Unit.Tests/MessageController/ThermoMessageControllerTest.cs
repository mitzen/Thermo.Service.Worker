using Xunit;
using Moq;
using Moq.AutoMock;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.MessageController;
using Service.ThermoDataModel.Models;
using Newtonsoft.Json;
using Service.ThermoDataModel;
using AzCloudApp.MessageProcessor.Core.DataProcessor;
using AzCloudApp.MessageProcessor.Core.EmailNotifier;
using System;

namespace AzCloudApp.MessageProcessor.Core.Unit.Tests.DataProcessor.MessageController
{
    public class ThermoMessageControllerTests
    {       
        [Fact]
        public async Task ProcessMessageSuccessfully()
        {
            var mocker = new AutoMocker();
            var fakeLogger = new Mock<ILogger>();

            var subject = mocker.CreateInstance<ThermoMessageController>();
            await subject.ProcessDataAsync(GetAttendanceData(CoreMessageType.AttendanceMessage), fakeLogger.Object);

            mocker.GetMock<IDataStoreProcesor>().Verify(x =>
            x.SaveAttendanceRecordAsync(It.IsAny<AttendanceRecord>()), Times.Once);

            mocker.GetMock<IDataFilter>().Verify(x =>
            x.ExecuteDataFiltering(It.IsAny<AttendanceRecord>(), fakeLogger.Object), Times.Once);
        }

        [Fact]
        public async Task ProcessTestMessageSuccessfully()
        {
            var mocker = new AutoMocker();
            var fakeLogger = new Mock<ILogger>();
            
            var subject = mocker.CreateInstance<ThermoMessageController>();
            await subject.ProcessDataAsync(GetAttendanceData("TEST"), fakeLogger.Object);

            fakeLogger.Verify(x => x.Log(
                      It.IsAny<LogLevel>(),
                      It.IsAny<EventId>(),
                      It.IsAny<It.IsAnyType>(),
                      It.IsAny<Exception>(),
                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ProcessHeartBeatMessageSuccessfully1()
        {
            var mocker = new AutoMocker();
            var fakeLogger = new Mock<ILogger>();

            var subject = mocker.CreateInstance<ThermoMessageController>();
            await subject.ProcessDataAsync(GetAttendanceData(CoreMessageType.HeartBeatMessage), fakeLogger.Object);

            fakeLogger.Verify(x => x.Log(
                      It.IsAny<LogLevel>(),
                      It.IsAny<EventId>(),
                      It.IsAny<It.IsAnyType>(),
                      It.IsAny<Exception>(),
                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Exactly(2));
        }


        private string GetAttendanceData(string messageType)
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

            return JsonConvert.SerializeObject(attendanceData);
        }
    }
}
