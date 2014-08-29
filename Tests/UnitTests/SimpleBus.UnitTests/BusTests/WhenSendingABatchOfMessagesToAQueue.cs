using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SimpleBus.Contract.Core;
using SimpleBus.Queue;
using SimpleBus.Subscription;
using SimpleBus.Topic;

namespace SimpleBus.UnitTests.BusTests
{
    [TestFixture]
    [Category("Unit Tests")]
    public class WhenSendingABatchOfMessagesToAQueue : SpecificationForAsync<Bus>
    {
        private IQueueMessageSender _queueMessageSender;

        protected override Task<Bus> Given()
        {
            _queueMessageSender = Substitute.For<IQueueMessageSender>();

            var logger = Substitute.For<ILogger>();
            var topicMessageSender = Substitute.For<ITopicMessageSender>();
            var queueMessageReceiver = Substitute.For<IQueueMessageDispatcher>();
            var subscriptionMessageReceiver = Substitute.For<ISubscriptionMessageDispatcher>();
            var subscriptionProcessorManager = Substitute.For<ISubscriptionMessageHandlerManager>();
            var queueProcessorManager = Substitute.For<IQueueMessageHandlerManager>();

            var bus = new Bus(logger, _queueMessageSender, topicMessageSender, queueMessageReceiver, subscriptionMessageReceiver, queueProcessorManager, subscriptionProcessorManager);
            return Task.FromResult(bus);
        }

        protected override async Task When()
        {
            IEnumerable<MessageToSend> messages = Enumerable.Range(1, 15).Select(i => new MessageToSend());
            await Subject.SendQueueMessageBatch(messages);
        }

        public class MessageToSend
        {
        }

        [Test]
        public void TheQueueMessageSenderShouldReceiveTheCall()
        {
            _queueMessageSender.Received().SendBatch(Arg.Any<IEnumerable<MessageToSend>>());
        }
    }
}