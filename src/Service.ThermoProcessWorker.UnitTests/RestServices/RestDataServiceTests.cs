using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using Service.ThermoDataModel.Requests;
using Moq.AutoMock;
using System;
using Moq;

namespace Service.ThermoProcessWorker.UnitTests.RestServices
{
    public class RestDataServiceTests : BaseTest
    {
        [Fact]
        public void UsingNsubstitute()
        {
            var fakeRequest = new Mock<IRestRequest>();

            var mocker = new AutoMocker();

            var target = mocker.CreateInstance<RestDataService>();

            var result = target.ExecuteAsync<AttendanceRequest>(fakeRequest.Object);

            mocker.GetMock<ILogger>().Verify(x => x.Log(
                      It.IsAny<LogLevel>(),
                      It.IsAny<EventId>(),
                      It.IsAny<It.IsAnyType>(),
                      It.IsAny<Exception>(),
                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);

        }
    }
}