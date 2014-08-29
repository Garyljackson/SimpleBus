using System.Threading.Tasks;

namespace SimpleBus.Contract.Core
{
    public interface ISubscriptionMessageHandler<in T> where T : class
    {
        Task HandleMessage(T message);
    }
}