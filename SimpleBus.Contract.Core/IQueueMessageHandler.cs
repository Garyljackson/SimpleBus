using System.Threading.Tasks;

namespace SimpleBus.Contract.Core
{
    public interface IQueueMessageHandler<in T> where T : class
    {
        Task HandleMessage(T message);
    }
}