using System;
using Microsoft.ServiceBus.Messaging;

namespace SimpleBus.Extensions
{
    internal static class BrokeredMessageExtentions
    {
        internal static string SafelyGetBodyTypeNameOrDefault(this BrokeredMessage message)
        {
            object name;
            return (message.Properties.TryGetValue(MessagePropertyKeys.MessageType, out name)
                ? (string) name
                : default(string));
        }

        internal static BrokeredMessage WithMessageType(this BrokeredMessage message, Type type)
        {
            message.Properties[MessagePropertyKeys.MessageType] = type.FullName;
            return message;
        }
    }
}