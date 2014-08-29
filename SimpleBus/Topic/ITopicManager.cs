using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBus.Topic
{
    internal interface ITopicManager
    {
        Task<MessageSender> GetSender(string topicIdentifier);
    }
}