using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;
using SimpleBus.Extensions;

namespace SimpleBus.Infrastructure
{
    internal class BrokeredMessageFactory : IBrokeredMessageFactory
    {
        private readonly ILogger _logger;
        private readonly ISerializer _serializer;

        public BrokeredMessageFactory(ILogger logger, ISerializer serializer)
        {
            _logger = logger;
            _serializer = serializer;
        }

        public BrokeredMessage Create<T>(T message) where T : class
        {
            _logger.Debug("Creating brokered message for type:{0}", typeof(T));

            BrokeredMessage brokeredMessage;

            if (message == null)
            {
                brokeredMessage = new BrokeredMessage();
            }
            else
            {
                byte[] messageBodyBytes = BuildBodyBytes(message);
                brokeredMessage = new BrokeredMessage(new MemoryStream(messageBodyBytes), true);

                brokeredMessage.WithMessageType(message.GetType());
            }

            return brokeredMessage;
        }

        public object GetBody(BrokeredMessage message)
        {
            Type receivedType = GetBodyType(message);

            byte[] bodyBytes;

            using (var dataStream = message.GetBody<Stream>())
            using (var memoryStream = new MemoryStream())
            {
                dataStream.CopyTo(memoryStream);
                bodyBytes = memoryStream.ToArray();
            }

            return _serializer.Deserialize(Encoding.UTF8.GetString(bodyBytes), receivedType);
        }


        private Type GetBodyType(BrokeredMessage message)
        {
            string typeName = message.SafelyGetBodyTypeNameOrDefault();

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = a.GetType(typeName, false, false);
                if (type != null)
                    return type;
            }

            throw new Exception(string.Format("Requested Type {0} Not found in any assemblies", typeName));
        }

        private byte[] BuildBodyBytes(object serializableObject)
        {
            if (serializableObject == null) throw new ArgumentNullException("serializableObject");

            string serialized = _serializer.Serialize(serializableObject);
            byte[] serializedBytes = Encoding.UTF8.GetBytes(serialized);
            return serializedBytes;
        }
    }
}