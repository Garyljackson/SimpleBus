using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using QueuePing.Receiver;
using QueuePing.Sender;
using SimpleBus.Configuration;
using SimpleBus.Contract.Core;
using SimpleBus.Logging;

namespace QueuePing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var exit = new ManualResetEvent(false);

            Task.Run(() => Start(exit));
            exit.WaitOne();
        }

        private static async void Start(EventWaitHandle exit)
        {
            IBus bus = await SetupBus();

            var pingSender = new PingSender(bus);

            Console.WriteLine("Enter some text to have it ponged back at you. Type 'exit' to quit...");

            while (true)
            {
                string input = Console.ReadLine();
                if (input.ToLowerInvariant() == "exit")
                {
                    exit.Set();
                }
                else
                {
                    await pingSender.SendPing(input);
                }
            }
        }

        private static async Task<IBus> SetupBus()
        {
            string connectionString = ConfigurationManager.AppSettings["AzureConnectionString"];
            var pingReceiver = new PingReceiver();

            IBus bus = new BusBuilder()
                .Configure()
                .WithConnectionString(connectionString)
                .RegisterQueueProcessor(pingReceiver)
                .WithLogger(new ConsoleLogger())
                .Build();

            await bus.Start();

            return bus;
        }
    }
}