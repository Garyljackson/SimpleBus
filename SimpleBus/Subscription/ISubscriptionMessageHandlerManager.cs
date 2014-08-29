using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleBus.Contract.Core;
using SimpleBus.Infrastructure;

namespace SimpleBus.Subscription
{
    internal interface ISubscriptionMessageHandlerManager
    {
        IReadOnlyCollection<KeyValuePair<TopicSubscriptionIdentifier, Func<object, Task>>> GetRegisteredHandlers { get; }

        void RegisterMessageHandler<T>(ISubscriptionMessageHandler<T> subscriptionMessageHandler) where T : class;

        void RegisterSubscriptionProcessorFactory<T>(Func<ISubscriptionMessageHandler<T>> subscriptionMessageHandlerFactory) where T : class;
    }
}