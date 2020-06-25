using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using Service.ThermoDataModel.Requests;
using NSubstitute;

namespace Service.ThermoProcessWorker.UnitTests.RestServices
{
    public class RestDataServiceTests : BaseTest
    {
        [Fact]
        public void UsingNsubstitute()
        {
            var client = Substitute.For<IRestClient>();
            var logger = Substitute.For<ILogger>();
            var request = Substitute.For<IRestRequest>();
            var response = Substitute.For<IRestResponse<AttendanceRequest>>();

            var target = new RestDataService(client, logger);
            var result = target.ExecuteAsync<AttendanceRequest>(request).Returns(response);

            logger.Received();
            client.Received();
        }
    }
}