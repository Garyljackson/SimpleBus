using System;
using Microsoft.ServiceBus;

namespace SimpleBus.Infrastructure
{
    internal class NamespaceManagerFactory : INamespaceManagerFactory
    {
        private readonly Func<NamespaceManager> _namespaceManager;

        public NamespaceManagerFactory(Func<NamespaceManager> namespaceManager)
        {
            _namespaceManager = namespaceManager;
        }

        public NamespaceManager Create()
        {
            return _namespaceManager();
        }
    }
}