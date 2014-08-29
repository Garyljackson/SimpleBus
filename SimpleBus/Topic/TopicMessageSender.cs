using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;
using SimpleBus.Infrastructure;

namespace SimpleBus.Topic
{
    internal class TopicMessageSender : ITopicMessageSender
    {
        private readonly ILogger _logger;
        private readonly IBrokeredMessageFactory _brokeredMessageFactory;
        private readonly IEndpointNamingPolicy _endpointNamingPolicy;
        private readonly ITopicManager _topicManager;

        public TopicMessageSender(ILogger logger, IBrokeredMessageFactory brokeredMessageFactory, ITopicManager topicManager,
            IEndpointNamingPolicy endpointNamingPolicy)
        {
            _logger = logger;
            _brokeredMessageFactory = brokeredMessageFactory;
            _topicManager = topicManager;
            _endpointNamingPolicy = endpointNamingPolicy;
        }

        public async Task Send<T>(T message) where T : class
        {
            Type messageType = typeof(T);
            string topicIdentifier = _endpointNamingPolicy.GetTopicName(messageType);

            MessageSender messageSender = await _topicManager.GetSender(topicIdentifier);
            BrokeredMessage brokeredMessage = _brokeredMessageFactory.Create(message);

            _logger.Debug("Sending topic message of type : {0}", messageType);
            await messageSender.SendAsync(brokeredMessage);
        }

        public async Task SendBatch<T>(IEnumerable<T> messages) where T : class
        {
            Type messageType = typeof(T);
            string topicIdentifier = _endpointNamingPolicy.GetTopicName(messageType);

            MessageSender messageSender = await _topicManager.GetSender(topicIdentifier);

            IEnumerable<BrokeredMessage> brokeredMessages = messages.Select(message => _brokeredMessageFactory.Create(message)).ToList();

            _logger.Debug("Sending a batch ({0}) of queue messages of type : {1}", messageType, brokeredMessages.Count());

            await messageSender.SendBatchAsync(brokeredMessages);
        }
    }
}