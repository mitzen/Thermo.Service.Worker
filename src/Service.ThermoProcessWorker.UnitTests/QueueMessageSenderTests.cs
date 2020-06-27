using RestSharp;
using Service.ThermoProcessWorker.UnitTests.AppBusinessLogic;
using Xunit;
using Microsoft.Extensions.Logging;
using Service.ThermoProcessWorker.RestServices;
using System.Threading;
using Service.ThermoDataModel.Requests;
using Service.MessageBusServiceProvider.Queue;
using System.Threading.Tasks;
using Moq.AutoMock;
using Moq;
using System;
using Microsoft.Azure.ServiceBus;

namespace Service.ThermoProcessWorker.UnitTests
{
    public class QueueMessageSenderTests
    {
        [Fact]
        public async Task WhenSendAsyncThenMessagesSentWithAzureClient()
        {
            var testMessages = "Test Message";
            var mocker = new AutoMocker();
            var target = mocker.CreateInstance<QueueMessageSender>();

            await target.SendMessagesAsync(testMessages);

            // Logging 
            mocker.GetMock<ILogger>().Verify(x => x.Log(
                      It.IsAny<LogLevel>(),
                      It.IsAny<EventId>(),
                      It.IsAny<It.IsAnyType>(),
                      It.IsAny<Exception>(),
                      (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), Times.Once);

            mocker.GetMock<IQueueClient>().Verify(x => x.SendAsync(It.IsAny<Message>()));
        }
    }
}
