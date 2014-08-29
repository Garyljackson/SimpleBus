using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;
using SimpleBus.Extensions;
using SimpleBus.Infrastructure;

namespace SimpleBus.Subscription
{
    internal class SubscriptionManager : ISubscriptionManager
    {
        private readonly ILogger _logger;

        /// The mananger caches receivers as per microsofts recommeded best practice
        /// http://msdn.microsoft.com/en-us/library/hh528527.aspx

        // Lazy is used to ensure that only one instance of the messageSender per type is created
        // Using a lazy with an initialisation method will also cache the exception if one occurs - http://msdn.microsoft.com/en-us/library/system.threading.lazythreadsafetymode(v=vs.110).aspx
        private readonly ConcurrentDictionary<string, AsyncLazy<MessageReceiver>> _messageReceiverCache = new ConcurrentDictionary<string, AsyncLazy<MessageReceiver>>();

        private readonly IMessageFactoryFactory _messageFactoryFactory;
        private readonly INamespaceManagerFactory _namespaceManagerFactory;

        public SubscriptionManager(ILogger logger, IMessageFactoryFactory messageFactoryFactory, INamespaceManagerFactory namespaceManagerFactory)
        {
            _messageFactoryFactory = messageFactoryFactory;
            _logger = logger;
            _namespaceManagerFactory = namespaceManagerFactory;
        }

        public async Task<MessageReceiver> GetReceiver(string subscriptionIdentifier, string topicIdentifier)
        {
            return await _messageReceiverCache.GetOrAdd(subscriptionIdentifier, type => ResolveSubscriptionReceiver(topicIdentifier, subscriptionIdentifier));
        }

        private AsyncLazy<MessageReceiver> ResolveSubscriptionReceiver(string topicIdentifier, string subscriptionIdentifier)
        {
            return new AsyncLazy<MessageReceiver>(async () =>
            {
                _logger.Debug("Resolving subscription sender:{0}", subscriptionIdentifier);
                await EnsureSubscriptionExists(topicIdentifier, subscriptionIdentifier);
                return await _messageFactoryFactory.Create().CreateMessageReceiverAsync(SubscriptionClient.FormatSubscriptionPath(topicIdentifier, subscriptionIdentifier));
            });
        }

        private Task EnsureSubscriptionExists(string topicIdentifier, string subscriptionIdentifier)
        {
            return _namespaceManagerFactory.Create().CreateSubscriptionIfNotExists(_logger, topicIdentifier, subscriptionIdentifier);
        }
    }
}