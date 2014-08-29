using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Configuration.Settings;
using SimpleBus.Contract.Core;
using SimpleBus.Infrastructure;

namespace SimpleBus.Subscription
{
    internal class SubscriptionMessageDispatcher : ISubscriptionMessageDispatcher
    {
        private readonly ILogger _logger;
        private readonly IBrokeredMessageFactory _brokeredMessageFactory;
        private readonly IEndpointNamingPolicy _endpointNamingPolicy;
        private readonly MaxConcurrentReceiverCallsSetting _maxConcurrentReceiverCallsSetting;
        private readonly List<MessageDispatcher> _messageDispatchers = new List<MessageDispatcher>();
        private readonly ISubscriptionManager _subscriptionManager;

        public SubscriptionMessageDispatcher(ILogger logger, ISubscriptionManager subscriptionManager, IBrokeredMessageFactory brokeredMessageFactory,
            IEndpointNamingPolicy endpointNamingPolicy, MaxConcurrentReceiverCallsSetting maxConcurrentReceiverCallsSetting)
        {
            _logger = logger;
            _brokeredMessageFactory = brokeredMessageFactory;
            _endpointNamingPolicy = endpointNamingPolicy;
            _maxConcurrentReceiverCallsSetting = maxConcurrentReceiverCallsSetting;
            _subscriptionManager = subscriptionManager;
        }

        public async Task StartDispatchers(IEnumerable<KeyValuePair<TopicSubscriptionIdentifier, Func<object, Task>>> typeHandlerMaps)
        {
            foreach (var typeHandlerMap in typeHandlerMaps)
            {
                string topicIdentifier = _endpointNamingPolicy.GetTopicName(typeHandlerMap.Key.MessageType);
                string subscriptionIdentifier = _endpointNamingPolicy.GetSubscriptionName(typeHandlerMap.Key.MessageType, typeHandlerMap.Key.SubscriberType);

                MessageReceiver receiver = await _subscriptionManager.GetReceiver(subscriptionIdentifier, topicIdentifier);

                Func<object, Task> processor = typeHandlerMap.Value;

                var dispatcher = new MessageDispatcher(_logger, receiver, processor, _brokeredMessageFactory, typeHandlerMap.Key.MessageType, _maxConcurrentReceiverCallsSetting);
                _messageDispatchers.Add(dispatcher);
            }
        }
    }
}