using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBus.Queue
{
    internal interface IQueueManager
    {
        Task<MessageReceiver> GetReceiver(string queueIdentifier);
        Task<MessageSender> GetSender(string queueIdentifier);
    }
}