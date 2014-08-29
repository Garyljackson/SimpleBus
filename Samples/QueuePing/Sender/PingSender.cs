using System.Threading.Tasks;
using QueuePing.Messages;
using SimpleBus.Contract.Core;

namespace QueuePing.Sender
{
    public class PingSender
    {
        private readonly IBus _bus;

        public PingSender(IBus bus)
        {
            _bus = bus;
        }


        public Task SendPing(string message)
        {
            return _bus.SendQueueMessage(new Ping {Data = message});
        }
    }
}