using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using System.Threading;
using Service.ThermoDataModel.Requests;
using Moq;
using Moq.AutoMock;
using Service.MessageBusServiceProvider.Queue;
using System.Threading.Tasks;

namespace Service.ThermoProcessWorker.UnitTests
{
    public class QueueMessageSenderTests
    {

        //[Fact]
        //public async Task If()
        //{
        //    var mocker = new AutoMocker();
        //    var target = mocker.CreateInstance<Account>();
        //    target.SayHi("leng chai");
        //    mocker.GetMock<IGreeting>().Verify(x => x.Greet(It.IsAny<string>()), Times.Once);

        //    //mocker.GetMock<ILogger>().Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
        //    //var target = mocker.CreateInstance<QueueMessageSender>();
        //    //var fakeRequest = new Mock<IRestRequest>();
        //    //await target.SendMessagesAsync("test");
        //    //mocker.GetMock<ILogger>().Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
        //    //var l = mocker.GetMock<ILogger>();
        //    //mocker.Verify<ILogger>((l) => l.LogInformation(It.IsAny<string>()), Times.Once);
        //    //l.Verify(l => l.LogInformation(It.IsAny<string>()), Times.Once);
        //    //l.Verify(l => l.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        //    //target.ExecuteAsync<AttendanceRequest>(fakeRequest.Object).GetAwaiter().GetResult();
        //    //var restclient = mocker.GetMock<IRestClient>();
        //    //restclient.Verify(r => restClient.ExecuteAsync<AttendanceRequest>(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        //    //var log = mocker.GetMock<ILogger>();
        //    //mocker.Verify<ILogger>((logger) => logger.LogInformation(It.IsAny<string>()), Times.Once);
        //    //mocker.Verify<IRestClient>((client) => restClient.ExecuteAsync<AttendanceRequest>(It.IsAny<AttendanceRequest>()), Times.Once);
        //}

        //[Fact]
        //public async Task WhenSayHiThenGreetMethodCalls()
        //{
        //    // arrange 
        //    var mocker = new AutoMocker();
        //    var target = mocker.CreateInstance<Account>();

        //    // act 
        //    target.SayHi("leng chai");

        //    // verify 
        //    mocker.GetMock<IGreeting>().Verify(x => x.Greet(It.IsAny<string>()), Times.Once);
        //}
    }
}
