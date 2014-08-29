using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleBus.Contract.Core;
using TopicFireHose.Messages;

namespace TopicFireHose.Sender
{
    public class FireHoseSender
    {
        private readonly IBus _bus;

        public FireHoseSender(IBus bus)
        {
            _bus = bus;
        }


        public async Task SprayMessages(int numberOfMessages)
        {
            IEnumerable<HotTopic> messages = 
                Enumerable.Range(1, numberOfMessages).Select(i => new HotTopic {MessageNumber = i, CreatedDateTimeUtc = DateTime.UtcNow});

            Console.WriteLine("Sending Messages");

            await _bus.SendTopicMessageBatch(messages);

            Console.WriteLine("Sending Messages Done");
        }
    }
}