using Microsoft.Extensions.Logging;
using RestSharp;
using Service.ThermoDataModel.Models;
using Service.ThermoProcessWorker.RestServices;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;

namespace Service.ThermoProcessWorker.UnitTests.RestServices
{
    public class ThermoDataRequesterTests : BaseTest
    {
        [Fact]
        public void If()
        {
            //var dataService = mocker.CreateInstance<IRestDataService>();
            //var logger = mocker.CreateInstance<ILogger>();
            //var request = mocker.CreateInstance<IRestRequest>();

            //var target = new ThermoDataRequester(dataService, logger);
            //var restResponse = target.GetAttendanceRecordAsync<AttendanceResponse>(request);
        }
    }
}
