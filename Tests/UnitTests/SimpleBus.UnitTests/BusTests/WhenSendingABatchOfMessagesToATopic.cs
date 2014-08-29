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
    public class WhenSendingABatchOfMessagesToATopic : SpecificationForAsync<Bus>
    {
        private ITopicMessageSender _topicMessageSender = Substitute.For<ITopicMessageSender>();


        protected override Task<Bus> Given()
        {
            var queueMessageSender = Substitute.For<IQueueMessageSender>();

            var logger = Substitute.For<ILogger>();
            _topicMessageSender = Substitute.For<ITopicMessageSender>();
            var queueMessageReceiver = Substitute.For<IQueueMessageDispatcher>();
            var subscriptionMessageReceiver = Substitute.For<ISubscriptionMessageDispatcher>();
            var subscriptionProcessorManager = Substitute.For<ISubscriptionMessageHandlerManager>();
            var queueProcessorManager = Substitute.For<IQueueMessageHandlerManager>();

            var bus = new Bus(logger, queueMessageSender, _topicMessageSender, queueMessageReceiver,
                subscriptionMessageReceiver, queueProcessorManager, subscriptionProcessorManager);
            return Task.FromResult(bus);
        }

        protected override async Task When()
        {
            IEnumerable<MessageToSend> messages = Enumerable.Range(1, 15).Select(i => new MessageToSend());
            await Subject.SendTopicMessageBatch(messages);
        }

        public class MessageToSend
        {
        }

        [Test]
        public void TheTopicMessageSenderShouldReceiveTheCall()
        {
            _topicMessageSender.Received().SendBatch(Arg.Any<IEnumerable<MessageToSend>>());
        }
    }
}