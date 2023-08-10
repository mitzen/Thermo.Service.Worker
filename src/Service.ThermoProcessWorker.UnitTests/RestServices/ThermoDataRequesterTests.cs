using Moq;
using Moq.AutoMock;
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
        public void WhenAttendanceCallThenRestDataServiceExecuteAsyncIsCalled()
        {
            var mocker = new AutoMocker();
            var fakeRequest = new Mock<IRestRequest>();

            ThermoDataRequester target = mocker.CreateInstance<ThermoDataRequester>();

            var result = target.GetAttendanceRecordAsync<AttendanceResponse>(fakeRequest.Object);

            mocker.GetMock<IRestDataService>().Verify(x => x.ExecuteAsync<AttendanceResponse>(fakeRequest.Object));
        }
    }
}
