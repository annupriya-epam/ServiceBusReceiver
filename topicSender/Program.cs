using Microsoft.Azure.ServiceBus;
using System;
using System.Text;
using System.Threading.Tasks;

namespace topicSender
{
    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://apbusservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=icjNJRIMehfL7UvrY6WONooRVFgt7yHv1tswtFbPAc0=";
        const string topicname = "topic1";
        static ITopicClient topicClient;
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            const int numberOfMessages = 10;
            topicClient = new TopicClient(ServiceBusConnectionString, topicname);
            await SendMessageAsync(numberOfMessages);
            await topicClient.CloseAsync();
        }

        static async Task SendMessageAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (int i = 0; i < numberOfMessagesToSend; i++)
                {
                    string messageBody = $"Message {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    message.Label = "test";
                    Console.WriteLine($"Sending Message : {messageBody}");
                    await topicClient.SendAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
