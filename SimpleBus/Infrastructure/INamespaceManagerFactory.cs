using Microsoft.ServiceBus;

namespace SimpleBus.Infrastructure
{
    internal interface INamespaceManagerFactory
    {
        NamespaceManager Create();
    }
}