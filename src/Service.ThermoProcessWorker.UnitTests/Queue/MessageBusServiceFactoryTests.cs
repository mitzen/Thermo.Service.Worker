using Xunit;
using Microsoft.Extensions.Logging;
using Service.MessageBusServiceProvider.Queue;
using Moq;
using System;

namespace Service.ThermoProcessWorker.UnitTests.Queue
{
    public class MessageBusServiceFactoryTest
    {
        private ServiceBusConfiguration incompletedConnectionString = new ServiceBusConfiguration
        {
            QueueName = "fakequeuename",
            ServiceBusConnection = "Endpoint=sb://devsbbank.servicebus.windows.net/;"
        };

        private ServiceBusConfiguration validConnectionString = new ServiceBusConfiguration
        {
            QueueName = "fakequeuename",
            ServiceBusConnection = "Endpoint=sb://devsbbank.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZeoCedTJSaqVPAx8bHX998DVIYHtuG5g0OKlUkUFF10="
        };

        [Fact]
        public void WhenConnectionGivenIsInvalidThrowsExceptions()
        { 
            Assert.Throws<ArgumentNullException>(() => MessageBusServiceFactory.CreateQueueClient(incompletedConnectionString));
        }

        [Fact]
        public void WhenConnectionGivenIsValidThenObjectReturnIsNotNull()
        {
            var target =  MessageBusServiceFactory.CreateQueueClient(validConnectionString);
            Assert.NotNull(target);
        }


        [Fact]
        public void WhenConnectionStringIsInvalidThenThrowsExceptionCreateClient()
        {
            var fakeLogger = new Mock<ILogger>();
            Assert.Throws<ArgumentNullException>(() => 
            MessageBusServiceFactory.CreateServiceBusMessageSender(incompletedConnectionString, fakeLogger.Object));
        }

        [Fact]
        public void WhenConnectionIsValidThenCreateServiceBusMessageSenderIsNotNull()
        {
            var fakeLogger = new Mock<ILogger>();
            var target  = MessageBusServiceFactory.CreateServiceBusMessageSender(validConnectionString, fakeLogger.Object);
            Assert.NotNull(target);
        }
    }
}
