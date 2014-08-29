using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleBus.Contract.Core;

namespace SimpleBus.Queue
{
    internal interface IQueueMessageHandlerManager
    {
        IReadOnlyCollection<KeyValuePair<Type, Func<object, Task>>> GetRegisteredHandlers { get; }

        void RegisterMessageHandler<T>(IQueueMessageHandler<T> queueMessageHandler) where T : class;

        void RegisterMessageHandlerFactory<T>(Func<IQueueMessageHandler<T>> queueMessageHandlerFactory) where T : class;
    }
}