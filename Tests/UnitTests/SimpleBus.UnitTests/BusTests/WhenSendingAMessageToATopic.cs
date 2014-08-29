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
    public class WhenSendingAMessageToATopic : SpecificationForAsync<Bus>
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
            await Subject.SendTopicMessage(new MessageToSend());
        }

        public class MessageToSend
        {
        }

        [Test]
        public void TheTopicMessageSenderShouldReceiveTheCall()
        {
            _topicMessageSender.Received().Send(Arg.Any<MessageToSend>());
        }
    }
}