using Microsoft.Azure.ServiceBus;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Queue.Mgmt.Dlq
{
    class Program
    {
        //const string ServiceBusConnectionString = "Endpoint=sb://devsbbank.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZeoCedTJSaqVPAx8bHX998DVIYHtuG5g0OKlUkUFF9g=";
        //const string QueueName = "devsbqbank/$deadletterqueue";
        static IQueueClient queueClient;
        private static AppArgument appArgument = new AppArgument();

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please provide endpoint connection string and target queue name.");
                Console.WriteLine("For example : ");
                Console.WriteLine("Endpoint=sb://devsbbank.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZeoCedTJSaqVPAx8bHX998DVIYHtuG5g0OKlUkUFF9g= devsbqbank/$deadletterqueue .\test.log");
            }

            appArgument.Endpoint = args[0];
            appArgument.TargetQueueName = $"{args[1]}/$deadletterqueue";
            appArgument.TargetFilePath = args[2] ?? $"{args[1]}-default-{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.log";
          
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            queueClient = new QueueClient(appArgument.Endpoint, appArgument.TargetQueueName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            // Register QueueClient's MessageHandler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();
            Console.ReadKey();
            await queueClient.CloseAsync();

        }
        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }


        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            using (StreamWriter writetext = new StreamWriter(appArgument.TargetFilePath))
            {
                writetext.WriteLine(message.Body);
            }

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

    }
}
