using Microsoft.ServiceBus.Messaging;

namespace SimpleBus.Infrastructure
{
    internal interface IMessageFactoryFactory
    {
        MessagingFactory Create();
    }
}