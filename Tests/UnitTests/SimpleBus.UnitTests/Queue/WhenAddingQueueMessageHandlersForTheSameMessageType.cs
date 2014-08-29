using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleBus.Contract.Core;
using SimpleBus.Queue;

namespace SimpleBus.UnitTests.Queue
{
    [TestFixture]
    public class WhenAddingQueueMessageHandlersForTheSameMessageType
    {
        [SetUp]
        public void Setup()
        {
            _queueMessageHandlerManager = new QueueMessageHandlerManager();
        }

        private QueueMessageHandlerManager _queueMessageHandlerManager;


        public class QueueMessageHandlerProcessor1 : IQueueMessageHandler<TestMessage>
        {
            public Task HandleMessage(TestMessage message)
            {
                throw new NotImplementedException();
            }
        }

        public class QueueMessageHandlerProcessor2 : IQueueMessageHandler<TestMessage>
        {
            public Task HandleMessage(TestMessage message)
            {
                throw new NotImplementedException();
            }
        }

        public class TestMessage
        {
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ItShouldFailWithException()
        {
            _queueMessageHandlerManager.RegisterMessageHandler(new QueueMessageHandlerProcessor1());
            _queueMessageHandlerManager.RegisterMessageHandler(new QueueMessageHandlerProcessor2());
        }
    }
}