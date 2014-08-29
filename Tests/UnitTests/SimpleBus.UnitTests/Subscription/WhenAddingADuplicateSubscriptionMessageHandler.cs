using System;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleBus.Contract.Core;
using SimpleBus.Subscription;

namespace SimpleBus.UnitTests.Subscription
{
    [TestFixture]
    public class WhenAddingADuplicateSubscriptionMessageHandler 
    {
        [SetUp]
        public void Setup()
        {
            _subscriptionMessageHandlerManager = new SubscriptionMessageHandlerManager();
        }

        private SubscriptionMessageHandlerManager _subscriptionMessageHandlerManager;


        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void ItShouldFailWithException()
        {
            _subscriptionMessageHandlerManager.RegisterMessageHandler(new Processor1());
            _subscriptionMessageHandlerManager.RegisterMessageHandler(new Processor1());
        }

        public class Processor1 : ISubscriptionMessageHandler<TestMessage>
        {
            public Task HandleMessage(TestMessage message)
            {
                throw new NotImplementedException();
            }
        }

        public class TestMessage
        {
        }

    }
}