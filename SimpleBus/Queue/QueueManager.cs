using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;
using SimpleBus.Extensions;
using SimpleBus.Infrastructure;

namespace SimpleBus.Queue
{
    internal class QueueManager : IQueueManager
    {
        private readonly ILogger _logger;

        /// The mananger caches senders and receivers as per microsofts recommeded best practice
        /// http://msdn.microsoft.com/en-us/library/hh528527.aspx

        // Lazy is used to ensure that only one instance of the messageSender per type is created
        // Using a lazy with an initialisation method will also cache the exception if one occurs - http://msdn.microsoft.com/en-us/library/system.threading.lazythreadsafetymode(v=vs.110).aspx
        private readonly ConcurrentDictionary<string, AsyncLazy<MessageReceiver>> _messageReceiverCache = new ConcurrentDictionary<string, AsyncLazy<MessageReceiver>>();
        private readonly ConcurrentDictionary<string, AsyncLazy<MessageSender>> _messageSenderCache = new ConcurrentDictionary<string, AsyncLazy<MessageSender>>();
        
        private readonly IMessageFactoryFactory _messageFactoryFactory;
        private readonly INamespaceManagerFactory _namespaceManagerFactory;

        public QueueManager(ILogger logger, IMessageFactoryFactory messageFactoryFactory, INamespaceManagerFactory namespaceManagerFactory)
        {
            _logger = logger;
            _messageFactoryFactory = messageFactoryFactory;
            _namespaceManagerFactory = namespaceManagerFactory;
        }

        public async Task<MessageReceiver> GetReceiver(string queueIdentifier)
        {
            return await _messageReceiverCache.GetOrAdd(queueIdentifier, type => ResolveQueueReceiver(queueIdentifier));
        }

        private AsyncLazy<MessageReceiver> ResolveQueueReceiver(string queueIdentifier)
        {
            return new AsyncLazy<MessageReceiver>(async () =>
            {
                _logger.Debug("Resolving queue receiver:{0}", queueIdentifier);
                await EnsureQueueExists(queueIdentifier);
                return await _messageFactoryFactory.Create().CreateMessageReceiverAsync(queueIdentifier);
            });
        }

        public async Task<MessageSender> GetSender(string queueIdentifier)
        {
            return await _messageSenderCache.GetOrAdd(queueIdentifier, type => ResolveQueueSender(queueIdentifier));
        }


        private AsyncLazy<MessageSender> ResolveQueueSender(string queueIdentifier)
        {
            return new AsyncLazy<MessageSender>(async () =>
            {
                _logger.Debug("Resolving queue sender:{0}", queueIdentifier);
                await EnsureQueueExists(queueIdentifier);
                return await _messageFactoryFactory.Create().CreateMessageSenderAsync(queueIdentifier);
            });
        }

        private Task EnsureQueueExists(string queueIdentifier)
        {
            return _namespaceManagerFactory.Create().CreateQueueIfNotExists(_logger, queueIdentifier);
        }
    }
}