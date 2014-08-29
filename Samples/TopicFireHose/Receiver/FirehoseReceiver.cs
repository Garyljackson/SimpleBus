using System;
using System.Threading;
using System.Threading.Tasks;
using SimpleBus.Contract.Core;
using TopicFireHose.Messages;

namespace TopicFireHose.Receiver
{
    public class FirehoseReceiver : ISubscriptionMessageHandler<HotTopic>
    {
        private readonly Random _randomDelayGenerator = new Random();

        public async Task HandleMessage(HotTopic message)
        {
            // Added a random delay to better demonstrate threading.
            var delay = _randomDelayGenerator.Next(1, 10);

            await Task.Delay(TimeSpan.FromSeconds(delay));
            
            Console.WriteLine("Processes message {0} on thread {1} with a {2} second delay", message.MessageNumber, Thread.CurrentThread.ManagedThreadId, delay);
        }
    }
}