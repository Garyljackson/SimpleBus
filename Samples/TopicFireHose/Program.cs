using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using SimpleBus.Configuration;
using SimpleBus.Contract.Core;
using SimpleBus.Logging;
using TopicFireHose.Receiver;
using TopicFireHose.Sender;

namespace TopicFireHose
{

    /// <summary>
    /// This program will send a user defined number of messages to a topic, which will then be processed async by the subscriber across a number of threads
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var exit = new ManualResetEvent(false);

            Task.Run(() => Start(exit));
            exit.WaitOne();

        }

        private static async Task Start(EventWaitHandle exit)
        {
            IBus bus = await SetupBus();
            
            var fireHoseSender = new FireHoseSender(bus);

            Console.WriteLine("How many messages would you like to spray?");

            while (true)
            {
                var consoleInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(consoleInput))
                {
                    Console.WriteLine("Please enter number or type 'Exit'");
                }
                else
                {
                    if (consoleInput.ToLowerInvariant() == "exit")
                    {
                        exit.Set();
                        break;
                    }

                    int numberOfMessages;

                    if (!int.TryParse(consoleInput, out numberOfMessages))
                    {
                        Console.WriteLine("Please enter a valid number");
                        continue;

                    }

                    await fireHoseSender.SprayMessages(numberOfMessages);
                    break;
                }
            }

            Console.ReadKey();
            exit.Set();
        }

        private static async Task<IBus> SetupBus()
        {
            string connectionString = ConfigurationManager.AppSettings["AzureConnectionString"];

            var subscriptionProcessor = new FirehoseReceiver();
            
            var bus = new BusBuilder()
                .Configure()
                .WithConnectionString(connectionString)
                .WithMaxConcurrentReceiverCallbacks(100)
                .RegisterSubscriptionProcessor(subscriptionProcessor)
                .WithLogger(new ConsoleLogger())
                .Build();


            await bus.Start();

            return bus;
        }

    }
}
