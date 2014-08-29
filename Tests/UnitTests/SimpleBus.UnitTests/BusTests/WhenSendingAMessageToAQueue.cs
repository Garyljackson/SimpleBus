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
    public class WhenSendingAMessageToAQueue : SpecificationForAsync<Bus>
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

            var bus = new Bus(logger, _queueMessageSender, topicMessageSender, queueMessageReceiver,
                subscriptionMessageReceiver, queueProcessorManager, subscriptionProcessorManager);
            return Task.FromResult(bus);
        }


        protected override async Task When()
        {
            await Subject.SendQueueMessage(new MessageToSend());
        }

        public class MessageToSend
        {
        }

        [Test]
        public void TheTopicMessageSenderShouldReceiveTheCall()
        {
            _queueMessageSender.Received().Send(Arg.Any<MessageToSend>());
        }
    }
}