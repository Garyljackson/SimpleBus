using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleBus.Contract.Core;
using SimpleBus.Infrastructure;

namespace SimpleBus.Subscription
{
    internal class SubscriptionMessageHandlerManager : ISubscriptionMessageHandlerManager
    {
        private readonly ConcurrentDictionary<TopicSubscriptionIdentifier, Func<object, Task>> _messageHandlers = new ConcurrentDictionary<TopicSubscriptionIdentifier, Func<object, Task>>();

        public IReadOnlyCollection<KeyValuePair<TopicSubscriptionIdentifier, Func<object, Task>>> GetRegisteredHandlers
        {
            get { return _messageHandlers.ToList().AsReadOnly(); }
        }

        public void RegisterMessageHandler<T>(ISubscriptionMessageHandler<T> subscriptionMessageHandler) where T : class
        {
            if(subscriptionMessageHandler==null)
                throw new ArgumentNullException("subscriptionMessageHandler");

            var topicSubscriptionIdentifier = new TopicSubscriptionIdentifier(subscriptionMessageHandler.GetType(), typeof (T));
            RegisterMessageHandler(topicSubscriptionIdentifier, message => subscriptionMessageHandler.HandleMessage((T) message));
        }

        public void RegisterSubscriptionProcessorFactory<T>(Func<ISubscriptionMessageHandler<T>> subscriptionMessageHandlerFactory) where T : class
        {
            if (subscriptionMessageHandlerFactory == null)
                throw new ArgumentNullException("subscriptionMessageHandlerFactory");

            var topicSubscriptionIdentifier = new TopicSubscriptionIdentifier(subscriptionMessageHandlerFactory.Method.ReturnType, typeof (T));
            RegisterMessageHandler(topicSubscriptionIdentifier, message => subscriptionMessageHandlerFactory().HandleMessage((T) message));
        }

        private void RegisterMessageHandler(TopicSubscriptionIdentifier topicSubscriptionIdentifier, Func<object, Task> messageHandler)
        {
            if (!_messageHandlers.TryAdd(topicSubscriptionIdentifier, messageHandler))
            {
                throw new InvalidOperationException(string.Format("A subscriber of type:{0}, has already been registered for messages of type:{1}", topicSubscriptionIdentifier.SubscriberType, topicSubscriptionIdentifier.MessageType));
            }
        }
    }
}