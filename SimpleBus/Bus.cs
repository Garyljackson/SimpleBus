using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleBus.Contract.Core;
using SimpleBus.Queue;
using SimpleBus.Subscription;
using SimpleBus.Topic;

namespace SimpleBus
{
    public class Bus : IBus
    {
        private readonly ILogger _logger;

        private readonly object _mutex = new object();
        private bool _isRunning;

        private readonly IQueueMessageHandlerManager _queueMessageHandlerManager;
        private readonly ISubscriptionMessageHandlerManager _subscriptionMessageHandlerManager;
        private readonly ITopicMessageSender _topicMessageSender;
        private readonly IQueueMessageSender _queueMessageSender;
        private readonly IQueueMessageDispatcher _queueMessageDispatcher;
        private readonly ISubscriptionMessageDispatcher _subscriptionMessageDispatcher;

        internal Bus(
            ILogger logger, IQueueMessageSender queueMessageSender, ITopicMessageSender topicMessageSender,
            IQueueMessageDispatcher queueMessageDispatcher, ISubscriptionMessageDispatcher subscriptionMessageDispatcher, 
            IQueueMessageHandlerManager queueMessageHandlerManager, ISubscriptionMessageHandlerManager subscriptionMessageHandlerManager)
        {
            _logger = logger;
            _queueMessageSender = queueMessageSender;
            _topicMessageSender = topicMessageSender;
            _queueMessageDispatcher = queueMessageDispatcher;
            _subscriptionMessageDispatcher = subscriptionMessageDispatcher;
            _queueMessageHandlerManager = queueMessageHandlerManager;
            _subscriptionMessageHandlerManager = subscriptionMessageHandlerManager;
        }


        public Task SendQueueMessage<T>(T message) where T : class
        {
            return _queueMessageSender.Send(message);
        }

        public Task SendQueueMessageBatch<T>(IEnumerable<T> messages) where T : class
        {
            return _queueMessageSender.SendBatch(messages);
        }

        public Task SendTopicMessage<T>(T message) where T : class
        {
            return _topicMessageSender.Send(message);
        }

        public Task SendTopicMessageBatch<T>(IEnumerable<T> messages) where T : class
        {
            return _topicMessageSender.SendBatch(messages);
        }

        public async Task Start()
        {
            lock (_mutex)
            {
                if (_isRunning) return;
                _isRunning = true;
            }

            _logger.Info("Bus starting...");

            try
            {
                await _queueMessageDispatcher.StartDispatchers(_queueMessageHandlerManager.GetRegisteredHandlers);
                await _subscriptionMessageDispatcher.StartDispatchers(_subscriptionMessageHandlerManager.GetRegisteredHandlers);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Bus failed to start");
                throw;
            }

            _logger.Info("Bus started.");

        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }


      
    }
}