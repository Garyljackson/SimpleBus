using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBus.Subscription
{
    internal interface ISubscriptionManager
    {
        Task<MessageReceiver> GetReceiver(string subscriptionIdentifier, string topicIdentifier);
    }
}