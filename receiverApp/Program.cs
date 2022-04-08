using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace receiverApp
{
    class Program
    {
        const string ServiceBusConnectionString = "";
        const string topicname = "topic1";
        const string subscriptionname = "subscription1";
        static ISubscriptionClient subscriptionClient;
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, topicname, subscriptionname);
            RegisterOnMessageHandlerAndReceiveMessage();
            Console.ReadKey();
            await subscriptionClient.CloseAsync();
        }

        static void RegisterOnMessageHandlerAndReceiveMessage()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            subscriptionClient.RegisterMessageHandler(ProcessMessageAsync, messageHandlerOptions);
        }
        static async Task ProcessMessageAsync(Message message, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Received Message : Sequence Number {message.SystemProperties.SequenceNumber}");
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs receivedEventArgs)
        {
            Console.WriteLine($"Message handler encounter exception {receivedEventArgs.Exception}");
            return Task.CompletedTask;

        }

    }
}
