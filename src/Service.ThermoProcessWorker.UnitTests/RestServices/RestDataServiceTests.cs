using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using Moq;

namespace Service.ThermoProcessWorker.UnitTests.RestServices
{
    public class RequestFactoryTest : BaseTest
    {
        [Fact]
        public void CreatedInstanceIsThermoDataRequester()
        {
            var fakeLogger = new Mock<ILogger>();

            var instance = RequestFactory.CreateRestService("http://localhost", fakeLogger.Object);
            Assert.NotNull(instance);
            Assert.IsType<ThermoDataRequester>(instance);
        }

        [Fact]
        public void CreatePostBodyRequestInstanceIsRestRequest()
        {
            var fakeLogger = new Mock<ILogger>();

            var instance = RequestFactory.CreatePostBodyRequest(
                "http://localhost", GetAttendanceData("Test"));
            
            Assert.NotNull(instance);
            Assert.IsType<RestRequest>(instance);
        }
    }
}