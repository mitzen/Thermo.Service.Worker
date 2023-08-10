using Xunit;
using Moq;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore;
using Microsoft.EntityFrameworkCore;
using AzCloudApp.MessageProcessor.Core.Thermo.DataStore.DataStoreModel;
using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Service.ThermoDataModel.Models;
using Service.ThermoDataModel.Heartbeat;
using System.Threading;

namespace AzCloudApp.MessageProcessor.Core.Unit.Tests.DataProcessor
{
    public class DataStoreMessageProcessorTest
    {
        [Fact]
        public async Task DataStoreMessageProcessorAbleStoreAttendanceRecord()
        {
            var mockSet = new Mock<DbSet<AttendanceDataStore>>();

            var mockContext = new Mock<ThermoDataContext>();
            mockContext.Setup(m => m.AttendanceRecord).Returns(mockSet.Object);
            
            var subject = new DataStoreMessageProcessor(mockContext.Object);
            await subject.SaveAttendanceRecordAsync(new AttendanceRecord());

            mockSet.Verify(m => m.Add(It.IsAny<AttendanceDataStore>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }       

        [Fact]
        public async Task DataStoreMessageProcessorAbleStoreHeartBeatRecord()
        {
            var mockSet = new Mock<DbSet<HeartBeatDataStore>>();

            var mockContext = new Mock<ThermoDataContext>();
            mockContext.Setup(m => m.HeartBeat).Returns(mockSet.Object);

            var subject = new DataStoreMessageProcessor(mockContext.Object);
            await subject.SaveHeartBeatRecordAsync(new HeartbeatMessage());

            mockSet.Verify(m => m.Update(It.IsAny<HeartBeatDataStore>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
