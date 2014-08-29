using Microsoft.ServiceBus.Messaging;

namespace SimpleBus.Infrastructure
{
    internal interface IBrokeredMessageFactory
    {
        BrokeredMessage Create<T>(T message) where T : class;
        object GetBody(BrokeredMessage message);
    }
}