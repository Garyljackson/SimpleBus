using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;
using SimpleBus.Infrastructure;

namespace SimpleBus.Queue
{
    internal class QueueMessageSender : IQueueMessageSender
    {
        private readonly IBrokeredMessageFactory _brokeredMessageFactory;
        private readonly IEndpointNamingPolicy _endpointNamingPolicy;
        private readonly ILogger _logger;
        private readonly IQueueManager _queueManager;

        public QueueMessageSender(ILogger logger, IBrokeredMessageFactory brokeredMessageFactory, IQueueManager queueManager, IEndpointNamingPolicy endpointNamingPolicy)
        {
            _brokeredMessageFactory = brokeredMessageFactory;
            _queueManager = queueManager;
            _endpointNamingPolicy = endpointNamingPolicy;
            _logger = logger;
        }

        public async Task Send<T>(T message) where T : class
        {
            Type messageType = typeof (T);
            string queueIdentifier = _endpointNamingPolicy.GetQueueName(messageType);

            MessageSender messageSender = await _queueManager.GetSender(queueIdentifier);
            BrokeredMessage brokeredMessage = _brokeredMessageFactory.Create(message);

            _logger.Debug("Sending queue message of type : {0}", messageType);

            await messageSender.SendAsync(brokeredMessage);
        }

        public async Task SendBatch<T>(IEnumerable<T> messages) where T : class
        {
            Type messageType = typeof (T);
            string queueIdentifier = _endpointNamingPolicy.GetQueueName(messageType);

            MessageSender messageSender = await _queueManager.GetSender(queueIdentifier);

            List<BrokeredMessage> brokeredMessages =
                messages.Select(message => _brokeredMessageFactory.Create(message)).ToList();

            _logger.Debug("Sending a batch ({0}) of queue messages of type : {1}", messageType, brokeredMessages.Count());

            await messageSender.SendBatchAsync(brokeredMessages);
        }
    }
}