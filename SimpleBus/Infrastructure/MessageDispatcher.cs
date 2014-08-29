using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Configuration.Settings;
using SimpleBus.Contract.Core;
using SimpleBus.Exceptions;

namespace SimpleBus.Infrastructure
{
    internal class MessageDispatcher
    {
        private readonly ILogger _logger;
        private readonly Func<object, Task> _messageProcessor;
        private readonly IBrokeredMessageFactory _brokeredMessageFactory;
        private readonly Type _inboundMessageType;

        internal MessageDispatcher(ILogger logger, MessageReceiver messageReceiver, Func<object, Task> messageProcessor, IBrokeredMessageFactory brokeredMessageFactory, Type inboundMessageType, MaxConcurrentReceiverCallsSetting maxConcurrentReceiverCallsSetting)
        {
            _logger = logger;
            _messageProcessor = messageProcessor;
            _brokeredMessageFactory = brokeredMessageFactory;
            _inboundMessageType = inboundMessageType;

            messageReceiver.OnMessageAsync(PreProcessMessage, new OnMessageOptions() { MaxConcurrentCalls = maxConcurrentReceiverCallsSetting });
        }

        private Task HandleMessage(object message)
        {
            return _messageProcessor(message);
        }

        private Task PreProcessMessage(BrokeredMessage brokeredMessage)
        {
            var bodyMessage = _brokeredMessageFactory.GetBody(brokeredMessage);

            if (!_inboundMessageType.IsInstanceOfType(bodyMessage))
            {
                var errorMessage = string.Format("There was a message type mismatch. Expected type: {0}, Received type: {1}", _inboundMessageType, bodyMessage.GetType());
                _logger.Error(errorMessage);
                
                throw new MessageDispatchException(errorMessage);
            }

            _logger.Debug("Dispatching a message of type: {0}", _inboundMessageType);

            return HandleMessage(bodyMessage);
        }

    }
}
