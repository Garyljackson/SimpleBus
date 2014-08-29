using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;

namespace SimpleBus.Extensions
{
    internal static class NamespaceManagerExtensions
    {
        public static async Task CreateQueueIfNotExists(this NamespaceManager namespaceManager, ILogger logger, string queueIdentifier)
        {
            if (await namespaceManager.QueueExistsAsync(queueIdentifier))
                return;

            try
            {
                logger.Debug("Creating Queue:{0}", queueIdentifier);

                var queueDescription = new QueueDescription(queueIdentifier);
                await namespaceManager.CreateQueueAsync(queueDescription);
            }
            catch (MessagingEntityAlreadyExistsException)
            {
                // the queue was created while we were trying to create it.
                logger.Warn("A queue with the name {0} already exists", queueIdentifier);
            }
        }

        public static async Task CreateTopicIfNotExists(this NamespaceManager namespaceManager, ILogger logger, string topicIdentifier)
        {
            if (await namespaceManager.TopicExistsAsync(topicIdentifier))
                return;

            try
            {
                logger.Debug("Creating topic:{0}", topicIdentifier);

                var topicDescription = new TopicDescription(topicIdentifier);
                await namespaceManager.CreateTopicAsync(topicDescription);
            }
            catch (MessagingEntityAlreadyExistsException)
            {
                // the topic was created while we were trying to create it.
                logger.Warn("A topic with the name {0} already exists", topicIdentifier);
            }
        }

        public static async Task CreateSubscriptionIfNotExists(this NamespaceManager namespaceManager, ILogger logger, string topicIdentifier,
            string subscriptionIdentifier)
        {
            if (await namespaceManager.SubscriptionExistsAsync(topicIdentifier, subscriptionIdentifier))
                return;

            await namespaceManager.CreateTopicIfNotExists(logger, topicIdentifier);

            try
            {
                logger.Debug("Creating subscription:{0} for topic:{1}", subscriptionIdentifier, topicIdentifier);

                var subscriptionDescription = new SubscriptionDescription(topicIdentifier, subscriptionIdentifier);
                await namespaceManager.CreateSubscriptionAsync(subscriptionDescription);
            }
            catch (MessagingEntityAlreadyExistsException)
            {
                // the subscription was created while we were trying to create it.
                logger.Warn("A subscription with the name {0} already exists", subscriptionIdentifier);
            }
        }

       
    }
}