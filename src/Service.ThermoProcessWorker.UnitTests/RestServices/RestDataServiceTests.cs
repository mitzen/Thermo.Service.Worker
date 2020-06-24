using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using System.Threading;
using Service.ThermoDataModel.Requests;
using Moq;

namespace Service.ThermoProcessWorker.UnitTests.RestServices
{
    public class RestDataServiceTests : BaseTest
    {
        private IRestClient restClient;
        private ILogger logger;
        private IRestRequest restRequest; 

        public RestDataServiceTests()
        {
            //restClient = mocker.Use<IRestClient>();
            //logger = mocker.CreateInstance<ILogger>();
            //restRequest = mocker.CreateInstance<IRestRequest>();
        }

        [Fact]
        public void If()
        {
            var target = mocker.CreateInstance<RestDataService>();
            //var log = mocker.Get<ILogger>(); 
            var fakerequest = mocker.CreateInstance<IRestRequest>();
            target.ExecuteAsync<AttendanceRequest>(fakerequest).GetAwaiter().GetResult();
            mocker.Verify<ILogger>((logger) => logger.LogInformation(It.IsAny<string>()), Times.Once);
            //mocker.Verify<IRestClient>((client) => restClient.ExecuteAsync<AttendanceRequest>(It.IsAny<AttendanceRequest>()), Times.Once);
        }
    }
}
