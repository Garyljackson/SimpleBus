using System;
using SimpleBus.Configuration.Settings;
using SimpleBus.Contract.Core;

namespace SimpleBus.Configuration
{
    public static class BusBuilderConfigurationExtensions
    {
        public static BusBuilderConfiguration WithConnectionString(this BusBuilderConfiguration configuration, string connectionString)
        {
            if(configuration==null)
                throw new ArgumentNullException("configuration");

            if(string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("A connectionString needs to be supplied", "connectionString");

            configuration.ConnectionString = new ConnectionStringSetting {Value = connectionString};
            return configuration;
        }


        public static BusBuilderConfiguration WithMaxConcurrentReceiverCallbacks(this BusBuilderConfiguration configuration, int maxConcurrentCallbacks)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            configuration.MaxConcurrentReceiverCalls = new MaxConcurrentReceiverCallsSetting { Value = maxConcurrentCallbacks };
            return configuration;
        }

        public static BusBuilderConfiguration WithLogger(this BusBuilderConfiguration configuration, ILogger logger)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if(logger==null)
                throw new ArgumentNullException("logger");

            configuration.Logger = logger;
            return configuration;
        }

        public static BusBuilderConfiguration RegisterQueueProcessor<T>(this BusBuilderConfiguration configuration, IQueueMessageHandler<T> queueMessageHandler) where T : class
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if(queueMessageHandler==null)
                throw new ArgumentNullException("queueMessageHandler");

            configuration.QueueMessageHandlers.RegisterMessageHandler(queueMessageHandler);

            return configuration;
        }

        public static BusBuilderConfiguration RegisterQueueProcessorFactory<T>(this BusBuilderConfiguration configuration, Func<IQueueMessageHandler<T>> queueProcessorFactory) where T : class
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (queueProcessorFactory == null)
                throw new ArgumentNullException("queueProcessorFactory");

            configuration.QueueMessageHandlers.RegisterMessageHandlerFactory(queueProcessorFactory);

            return configuration;
        }

        public static BusBuilderConfiguration RegisterSubscriptionProcessor<T>(this BusBuilderConfiguration configuration, ISubscriptionMessageHandler<T> subscriptionMessageHandler) where T : class 
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (subscriptionMessageHandler == null)
                throw new ArgumentNullException("subscriptionMessageHandler");

            configuration.SubscriptionMessageHandlers.RegisterMessageHandler(subscriptionMessageHandler);
            
            return configuration;
        }

        public static BusBuilderConfiguration RegisterSubscriptionProcessorFactory<T>(this BusBuilderConfiguration configuration, Func<ISubscriptionMessageHandler<T>> subscriptionProcessorFactory) where T : class
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            if (subscriptionProcessorFactory == null)
                throw new ArgumentNullException("subscriptionProcessorFactory");

            configuration.SubscriptionMessageHandlers.RegisterSubscriptionProcessorFactory(subscriptionProcessorFactory);

            return configuration;
        }
    }
}