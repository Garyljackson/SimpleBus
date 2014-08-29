using System.Reflection;
using Autofac;
using Autofac.Core.Activators.Reflection;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using SimpleBus.Contract.Core;
using SimpleBus.Infrastructure;
using SimpleBus.Queue;
using SimpleBus.Subscription;
using SimpleBus.Topic;

namespace SimpleBus.Configuration
{
    public class BusBuilder
    {
        public BusBuilderConfiguration Configure()
        {
            return new BusBuilderConfiguration();
        }

        internal static IBus Build(BusBuilderConfiguration busBuilderConfiguration)
        {
            var builder = new ContainerBuilder();

            RegisterConfiguration(builder, busBuilderConfiguration);
            RegisterMessageProcessors(builder, busBuilderConfiguration);

            builder.RegisterType<Bus>()
                .AsImplementedInterfaces()
                .FindConstructorsWith(
                    new DefaultConstructorFinder(
                        type => type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)))
                .SingleInstance();

            builder.RegisterType<QueueMessageSender>().AsImplementedInterfaces();
            builder.RegisterType<TopicMessageSender>().AsImplementedInterfaces();
            builder.RegisterType<QueueMessageDispatcher>().AsImplementedInterfaces();
            builder.RegisterType<SubscriptionMessageDispatcher>().AsImplementedInterfaces();

            builder.RegisterType<BrokeredMessageFactory>().AsImplementedInterfaces();
            builder.RegisterType<QueueManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<TopicManager>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<MessageFactoryFactory>().AsImplementedInterfaces();
            builder.RegisterType<NamespaceManagerFactory>().AsImplementedInterfaces();


            builder.Register(
                context => MessagingFactory.CreateFromConnectionString(busBuilderConfiguration.ConnectionString))
                .AsSelf()
                .SingleInstance();

            builder.Register(
                context => NamespaceManager.CreateFromConnectionString(busBuilderConfiguration.ConnectionString))
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<SimpleEndpointNamingPolicy>().AsImplementedInterfaces();

            builder.RegisterType<JsonSerializer>().AsImplementedInterfaces();

            builder.RegisterType<SubscriptionManager>().AsImplementedInterfaces().SingleInstance();

            IContainer container = builder.Build();

            var bus = container.Resolve<IBus>();


            return bus;
        }


        private static void RegisterConfiguration(ContainerBuilder builder, BusBuilderConfiguration busBuilderConfiguration)
        {
            builder.RegisterInstance(busBuilderConfiguration.ConnectionString);
            builder.RegisterInstance(busBuilderConfiguration.MaxConcurrentReceiverCalls);
            builder.RegisterInstance(busBuilderConfiguration.Logger).As<ILogger>().SingleInstance();
        }

        private static void RegisterMessageProcessors(ContainerBuilder builder, BusBuilderConfiguration busBuilderConfiguration)
        {
            builder.RegisterInstance(busBuilderConfiguration.QueueMessageHandlers).As<IQueueMessageHandlerManager>().SingleInstance();
            builder.RegisterInstance(busBuilderConfiguration.SubscriptionMessageHandlers).As<ISubscriptionMessageHandlerManager>().SingleInstance();
        }

    }
}