using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using System.Threading;
using Service.ThermoDataModel.Requests;

namespace Service.ThermoProcessWorker.UnitTests.RestServices
{
    public class RestDataServiceTests : BaseTest
    {
        private IRestClient restClient;
        private ILogger logger;
        private IRestRequest restRequest; 

        public RestDataServiceTests()
        {
            restClient = mocker.CreateInstance<IRestClient>();
            logger = mocker.CreateInstance<ILogger>();
            restRequest = mocker.CreateInstance<IRestRequest>();
        }

        [Fact]
        public void If()
        {
            var target = new RestDataService(restClient, new CancellationToken(), logger);
            var result = target.ExecuteAsync<AttendanceRequest>(restRequest);

        }
    }
}
