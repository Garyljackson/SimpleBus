using System;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBus.Infrastructure
{
    internal class MessageFactoryFactory : IMessageFactoryFactory
    {
        private readonly Func<MessagingFactory> _messagingFactory;

        public MessageFactoryFactory(Func<MessagingFactory> messagingFactory)
        {
            _messagingFactory = messagingFactory;
        }

        public MessagingFactory Create()
        {
            return _messagingFactory();
        }
    }
}