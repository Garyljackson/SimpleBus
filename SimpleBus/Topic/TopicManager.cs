using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;
using SimpleBus.Extensions;
using SimpleBus.Infrastructure;

namespace SimpleBus.Topic
{
    internal class TopicManager : ITopicManager
    {
       private readonly ILogger _logger;

        /// The mananger caches sender as per microsofts recommeded best practice
        /// http://msdn.microsoft.com/en-us/library/hh528527.aspx

        // Lazy is used to ensure that only one instance of the messageSender per type is created
        // Using a lazy with an initialisation method will also cache the exception if one occurs - http://msdn.microsoft.com/en-us/library/system.threading.lazythreadsafetymode(v=vs.110).aspx
        private readonly ConcurrentDictionary<string, AsyncLazy<MessageSender>> _messageSenderCache = new ConcurrentDictionary<string, AsyncLazy<MessageSender>>();

        private readonly IMessageFactoryFactory _messageFactoryFactory;
        private readonly INamespaceManagerFactory _namespaceManagerFactory;

        public TopicManager(ILogger logger, IMessageFactoryFactory messageFactoryFactory, INamespaceManagerFactory namespaceManagerFactory)
        {
            _logger = logger;
            _messageFactoryFactory = messageFactoryFactory;
            _namespaceManagerFactory = namespaceManagerFactory;
        }

        public async Task<MessageSender> GetSender(string topicIdentifier)
        {
            return await _messageSenderCache.GetOrAdd(topicIdentifier, type => ResolveTopicSender(topicIdentifier));
        }

        private AsyncLazy<MessageSender> ResolveTopicSender(string topicIdentifier)
        {
            return new AsyncLazy<MessageSender>(async () =>
            {
                _logger.Debug("Resolving topic sender:{0}", topicIdentifier);
                await EnsureTopicExists(topicIdentifier);
                return await _messageFactoryFactory.Create().CreateMessageSenderAsync(topicIdentifier);
            });
        }

        private Task EnsureTopicExists(string topicIdentifier)
        {
            return _namespaceManagerFactory.Create().CreateTopicIfNotExists(_logger, topicIdentifier);
        }
    }
}