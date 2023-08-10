using Xunit;
using Moq;
using AzCloudApp.MessageProcessor.Core.DataProcessor;
using Moq.AutoMock;
using Microsoft.Extensions.Logging;
using Service.ThermoDataModel.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzCloudApp.MessageProcessor.Core.Unit.Tests.DataProcessor
{
    public class NotificationMessageProcessorTest
    {
        [Fact]
        public async Task MessageProcessorAbleToSendMail()
        {
            var mocker = new AutoMocker();
            var fakeLogger = new Mock<ILogger>();
            var subject = mocker.CreateInstance<NotificationMessageProcessor>();                      
            await subject.ProcessAsync(GetMailMessage(), fakeLogger.Object);
            mocker.GetMock<ISendMailService>().Verify(x => x.SendMailAsync(It.IsAny<MailContentData>(),
                It.IsAny<ILogger>()), Times.Once);
        }

        [Fact]
        public async Task MessageProcessorSendInvalidContentFail()
        {
            var mocker = new AutoMocker();
            var fakeLogger = new Mock<ILogger>();
            var subject = mocker.CreateInstance<NotificationMessageProcessor>();

            await subject.ProcessAsync(string.Empty, fakeLogger.Object);
            mocker.GetMock<ISendMailService>().Verify(x => x.SendMailAsync(It.IsAny<MailContentData>(),
                It.IsAny<ILogger>()), Times.Never);
        }

        private string GetMailMessage()
        {
            var mailMessage = new MailContentData();
            mailMessage.MailInfo = new MailInfo
            {
                ContentBody = "test content",
                Recipients = new List<string>() { "kepung@gmail.com" },
                Sender = "noreply@test.com"
            };
            return JsonConvert.SerializeObject(mailMessage);
        }
    }
}
