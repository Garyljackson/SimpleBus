using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBus.Queue
{
    internal interface IQueueMessageSender
    {
        Task Send<T>(T message) where T : class;
        Task SendBatch<T>(IEnumerable<T> messages) where T : class;
    }
}