using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBus.Contract.Core
{
    public interface IBus
    {
        Task SendQueueMessage<T>(T message) where T : class;
        Task SendQueueMessageBatch<T>(IEnumerable<T> messages) where T : class;
        Task SendTopicMessage<T>(T message) where T : class;
        Task SendTopicMessageBatch<T>(IEnumerable<T> messages) where T : class;
        Task Start();
        Task Stop();
    }
}