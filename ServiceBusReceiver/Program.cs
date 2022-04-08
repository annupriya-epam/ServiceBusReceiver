using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusReceiver
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://messgaeserviceevent.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=9GYQTZg7LZ8ZtEZIaWfcNdiKTyWg3VtbYGU7WVjoiU0=";
        const string queuename = "queue1";
        static IQueueClient queueClient;
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, queuename);
             RegisterOnMessageHandlerAndReceiveMessage();
            Console.ReadKey();
            await queueClient.CloseAsync();
        }
        static void RegisterOnMessageHandlerAndReceiveMessage()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            queueClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }
        static async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            throw new Exception();
            Console.WriteLine($"Received Message : Sequence Number {message.SystemProperties.SequenceNumber}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

        }

        static  Task ExceptionReceivedHandler(ExceptionReceivedEventArgs receivedEventArgs)
        {
            Console.WriteLine("handled");
            Console.WriteLine($"Message handler encounter exception {receivedEventArgs.Exception}");
            return Task.CompletedTask;

        }
    }
}
