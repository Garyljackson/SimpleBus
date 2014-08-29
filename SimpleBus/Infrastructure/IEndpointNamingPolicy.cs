using System;

namespace SimpleBus.Infrastructure
{
    internal interface IEndpointNamingPolicy
    {
        string GetQueueName(Type messageType);
        string GetTopicName(Type messageType);
        string GetSubscriptionName(Type messageType, Type processorType);
    }
}