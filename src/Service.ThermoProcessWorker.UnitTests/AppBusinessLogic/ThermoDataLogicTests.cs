using Xunit;
using Moq;
using Moq.AutoMock;
using System.Threading;
using Service.ThermoProcessWorker.AppBusinessLogic;
using Microsoft.Extensions.Configuration;

namespace Service.ThermoProcessWorker.UnitTests.AppBusinessLogic
{
    public class ThermoDataLogicTests : BaseTest
    {
        [Fact]
        public void ThermoLogicExecutionStarted()
        {  
            var mocker = new AutoMocker();

            var section = new Mock<IConfigurationSection>();
            mocker.GetMock<IConfiguration>().Setup(x =>
            x.GetSection(It.IsAny<string>())).Returns(section.Object);

            var subject = mocker.CreateInstance<BackgroundRestWorkerService>();

            var cancellationToken = new CancellationToken();
            subject.StartAsync(cancellationToken);

            mocker.GetMock<IThermoDataLogic>().Verify(x => x.Setup(It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
