using System;
using System.Linq;
using SimpleBus.Configuration.Settings;
using SimpleBus.Contract.Core;
using SimpleBus.Exceptions;
using SimpleBus.Infrastructure;
using SimpleBus.Queue;
using SimpleBus.Subscription;

namespace SimpleBus.Configuration
{
    public class BusBuilderConfiguration
    {
        internal readonly QueueMessageHandlerManager QueueMessageHandlers;
        internal readonly SubscriptionMessageHandlerManager SubscriptionMessageHandlers;
        internal ILogger Logger;

        public BusBuilderConfiguration()
        {
            QueueMessageHandlers = new QueueMessageHandlerManager();
            SubscriptionMessageHandlers = new SubscriptionMessageHandlerManager();
        }

        internal ConnectionStringSetting ConnectionString { get; set; }
        internal MaxConcurrentReceiverCallsSetting MaxConcurrentReceiverCalls { get; set; }
        
        public IBus Build()
        {
            AssertConfigurationIsValid();
            return BusBuilder.Build(this);
        }

        private void AssertConfigurationIsValid()
        {
      
            if(MaxConcurrentReceiverCalls==null)
                MaxConcurrentReceiverCalls = new MaxConcurrentReceiverCallsSetting();

            if (Logger == null)
                Logger = new NullLogger();

            var validatableComponents = GetType().GetProperties()
                                                 .Select(p => p.GetValue(this))
                                                 .OfType<IValidatableConfigurationSetting>()
                                                 .ToArray();

            var validationErrors = validatableComponents
                .SelectMany(c => c.Validate())
                .ToArray();

            if (!validationErrors.Any()) return;

            var message = string.Join(Environment.NewLine, new[] { "Bus configuration is invalid:" }.Concat(validationErrors));
            throw new BusException(message);

        }
    }
}