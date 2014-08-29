using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Configuration.Settings;
using SimpleBus.Contract.Core;
using SimpleBus.Infrastructure;

namespace SimpleBus.Queue
{
    internal class QueueMessageDispatcher : IQueueMessageDispatcher
    {
        private readonly IBrokeredMessageFactory _brokeredMessageFactory;
        private readonly IEndpointNamingPolicy _endpointNamingPolicy;
        private readonly MaxConcurrentReceiverCallsSetting _maxConcurrentReceiverCallsSetting;
        private readonly List<MessageDispatcher> _messageDispatchers = new List<MessageDispatcher>();
        private readonly ILogger _logger;
        private readonly IQueueManager _queueManager;

        public QueueMessageDispatcher(ILogger logger, IQueueManager queueManager, IBrokeredMessageFactory brokeredMessageFactory,
            IEndpointNamingPolicy endpointNamingPolicy, MaxConcurrentReceiverCallsSetting maxConcurrentReceiverCallsSetting)
        {
            _logger = logger;
            _queueManager = queueManager;
            _brokeredMessageFactory = brokeredMessageFactory;
            _endpointNamingPolicy = endpointNamingPolicy;
            _maxConcurrentReceiverCallsSetting = maxConcurrentReceiverCallsSetting;
        }

        public async Task StartDispatchers(IEnumerable<KeyValuePair<Type, Func<object, Task>>> typeHandlerMaps)
        {
            foreach (var typeHandlerMap in typeHandlerMaps)
            {
                string queueIdentifier = _endpointNamingPolicy.GetQueueName(typeHandlerMap.Key);

                MessageReceiver receiver = await _queueManager.GetReceiver(queueIdentifier);
                Func<object, Task> processor = typeHandlerMap.Value;

                var dispatcher = new MessageDispatcher(_logger, receiver, processor, _brokeredMessageFactory, typeHandlerMap.Key, _maxConcurrentReceiverCallsSetting);
                _messageDispatchers.Add(dispatcher);
            }
        }
    }
}