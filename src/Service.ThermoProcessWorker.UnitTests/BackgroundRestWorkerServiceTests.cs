using Xunit;
using Moq;
using Moq.AutoMock;
using System.Threading;
using Service.ThermoProcessWorker.AppBusinessLogic;
using Microsoft.Extensions.Configuration;
using Service.ThermoDataModel.Configuration;
using Service.MessageBusServiceProvider.CheckPointing;
using Microsoft.Extensions.Logging;
using System;

namespace Service.ThermoProcessWorker.UnitTests
{
    public class BackgroundRestWorkerServiceTests
    {
        [Fact]
        public void ServiceWorkerConfigrationIsNullTest()
        {
            var mocker = new AutoMocker();

            var cfg = new ServiceWorkerConfiguration()
            {
                GetDataFromRestServiceIntervalSecond = 10
            };

            var configuration = new Mock<IConfiguration>();

            var oneSectionMock = new Mock<IConfigurationSection>();
            oneSectionMock.Setup(s => s.Value).Returns("1");

            var twoSectionMock = new Mock<IConfigurationSection>();
            twoSectionMock.Setup(s => s.Value).Returns("2");

            var fooBarSectionMock = new Mock<IConfigurationSection>();
                     
            configuration.Setup(c => c.GetSection("ServiceWorkerConfiguration")).Returns(fooBarSectionMock.Object);

            var l = new Mock<ILogger<BackgroundRestWorkerService>>();
            var c = new Mock<ICheckPointLogger>();
            var t = new Mock<IThermoDataLogic>();
           
            var subject = new BackgroundRestWorkerService(l.Object, t.Object, configuration.Object,
                c.Object);

            _ = Assert.ThrowsAsync<NullReferenceException>(async () =>
            await subject.StartAsync(new CancellationToken()));                
         
        }
    }
}
