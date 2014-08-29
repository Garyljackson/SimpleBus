using System;

namespace SimpleBus.Infrastructure
{
    internal struct TopicSubscriptionIdentifier
    {
        private readonly Type _messageType;
        private readonly Type _subscriberType;

        public TopicSubscriptionIdentifier(Type subscriberType, Type messageType)
        {
            _subscriberType = subscriberType;
            _messageType = messageType;
        }

        public Type SubscriberType
        {
            get { return _subscriberType; }
        }

        public Type MessageType
        {
            get { return _messageType; }
        }

    }
}