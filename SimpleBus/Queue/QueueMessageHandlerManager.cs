using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleBus.Contract.Core;

namespace SimpleBus.Queue
{
    internal class QueueMessageHandlerManager : IQueueMessageHandlerManager
    {
        private readonly ConcurrentDictionary<Type, Func<object, Task>> _messageHandlers = new ConcurrentDictionary<Type, Func<object, Task>>();

        public IReadOnlyCollection<KeyValuePair<Type, Func<object, Task>>> GetRegisteredHandlers
        {
            get { return _messageHandlers.ToList().AsReadOnly(); }
        }

        public void RegisterMessageHandler<T>(IQueueMessageHandler<T> queueMessageHandler) where T : class
        {
            if (queueMessageHandler == null)
                throw new ArgumentNullException("queueMessageHandler");

            RegisterMessageHandler(typeof (T), message => queueMessageHandler.HandleMessage((T) message));

        }

        public void RegisterMessageHandlerFactory<T>(Func<IQueueMessageHandler<T>> queueMessageHandlerFactory) where T : class
        {
            if (queueMessageHandlerFactory == null)
                throw new ArgumentNullException("queueMessageHandlerFactory");

            RegisterMessageHandler(typeof(T), message => queueMessageHandlerFactory().HandleMessage((T)message));

        }

        private void RegisterMessageHandler(Type messageType, Func<object, Task> messageHandler)
        {
            if (!_messageHandlers.TryAdd(messageType, messageHandler))
            {
                throw new InvalidOperationException(string.Format("There is already a queue messageHandler registered for the message type:{0}", messageType));
            }
        }

    }
}