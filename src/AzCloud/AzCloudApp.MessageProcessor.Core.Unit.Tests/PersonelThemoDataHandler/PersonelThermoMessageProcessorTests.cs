using Xunit;
using Moq;
using Moq.AutoMock;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AzCloudApp.MessageProcessor.Core.PersonelThemoDataHandler;
using AzCloudApp.MessageProcessor.Core.MessageController;

namespace AzCloudApp.MessageProcessor.Core.Unit.Tests.DataProcessor
{
    public class PersonelThermoMessageProcessorTests
    {
        [Fact]
        public async Task PersonelThermoMessageProcessorSendMessage()
        {
            var mocker = new AutoMocker();
            var fakeLogger = new Mock<ILogger>();

            var message = "mytestmessage";

            var subject = mocker.CreateInstance<PersonelThermoMessageProcessor>();                      
            await subject.ProcessMessage(message, fakeLogger.Object);

            mocker.GetMock<IMessageController>().Verify(x => x.ProcessDataAsync(It.IsAny<string>(),
                It.IsAny<ILogger>()), Times.Once);
        }   
    }
}
