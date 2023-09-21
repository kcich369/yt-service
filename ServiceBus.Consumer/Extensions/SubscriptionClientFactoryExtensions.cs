using Domain.Configurations;
using Microsoft.Azure.ServiceBus;
using ServiceBus.Producer.Enumeration;

namespace ServiceBus.Consumer.Extensions;

public static class SubscriptionClientFactoryExtensions
{
    public static SubscriptionClient CreateSubscriptionClient(this AzureServiceBusConfiguration configuration, EventsNamesEnums value) =>
        new (configuration.ConnectionString, configuration.TopicName, value.Name);
}