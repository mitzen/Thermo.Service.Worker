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
