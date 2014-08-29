using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBus.Topic
{
    internal interface ITopicMessageSender
    {
        Task Send<T>(T message) where T : class;
        Task SendBatch<T>(IEnumerable<T> messages) where T : class;
    }
}