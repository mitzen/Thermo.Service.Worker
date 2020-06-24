using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using Service.ThermoDataModel.Requests;
using Moq;
using Moq.AutoMock;
using System;

namespace Service.ThermoProcessWorker.UnitTests.RestServices
{
    public class RestDataServiceTests : BaseTest
    {
        [Fact]
        public async void WhenRestServiceRequestAttendanceThenReturnsAttendanceResponse()
        {
            var target = mocker.CreateInstance<RestDataService>();
            var fakeRequest = new Mock<IRestRequest>();
         
            await target.ExecuteAsync<AttendanceRequest>(fakeRequest.Object);
         
            mocker.GetMock<ILogger>().Verify(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);
        }

        [Fact]
        public void IfTraditionalMock()
        {
            var target = mocker.CreateInstance<RestDataService>();
            var fakeRequest = new Mock<IRestRequest>();
            //mocker.GetMock<IRestClient>().Setup(c => c.ExecuteAsync<AttendanceRequest>(fakeRequest.Object)).Returns()
            //await target.ExecuteAsync<AttendanceRequest>(fakeRequest.Object);
        }
    }
}

