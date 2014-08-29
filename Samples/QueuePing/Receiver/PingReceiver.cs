using System;
using System.Threading.Tasks;
using QueuePing.Messages;
using SimpleBus.Contract.Core;

namespace QueuePing.Receiver
{
    public class PingReceiver : IQueueMessageHandler<Ping>
    {
        public Task HandleMessage(Ping message)
        {
            Console.WriteLine("Ping Message Received : {0} at {1}", message.Data, DateTime.Now);
            return Task.FromResult(true);
        }
    }
}