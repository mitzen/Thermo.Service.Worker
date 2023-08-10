using Xunit;
using Moq;
using Moq.AutoMock;
using Microsoft.Extensions.Logging;
using AzCloudApp.MessageProcessor.Core.EmailNotifier.Utils;
using AzCloudApp.MessageProcessor.Core.EmailNotifier;
using Microsoft.Extensions.Options;

namespace AzCloudApp.MessageProcessor.Core.Unit.Tests.EmailNotifier.Utils
{
    public class MailContentParserTests
    {
        private const string subjectMessage = "Test";
        private const string senderContent = "Test";
        private const string DeviceId = "testdeviceid";
        private const string EmailMessage = "email test messsage";

        [Fact]
        public void ParseMailContentTest()
        {
            var mocker = new AutoMocker();
            var mockLogger = new Mock<ILogger>();

            var infoParameter = new EmailTemperatureHitParameter(DeviceId, EmailMessage);
            var temperatureConfig = new TemperatureFilterConfiguration()
            {
                  Subject = subjectMessage,
                  Sender = senderContent,
                  Max = 37d
            };

            mocker.GetMock<IOptions<TemperatureFilterConfiguration>>().Setup(x =>
            x.Value).Returns(temperatureConfig);
            
            var subject = mocker.CreateInstance<MailContentParser>();                     
            var result = subject.CreateTemperatureEmailAlertMessage(infoParameter, mockLogger.Object);

            Assert.NotNull(result);
            Assert.Equal(subjectMessage, result.MailInfo.Subject);
            Assert.Equal(senderContent, result.MailInfo.Sender);
        }
    }
}
