using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleBus.Infrastructure;

namespace SimpleBus.Subscription
{
    internal interface ISubscriptionMessageDispatcher
    {
        Task StartDispatchers(IEnumerable<KeyValuePair<TopicSubscriptionIdentifier, Func<object, Task>>> typeHandlerMaps);
    }
}