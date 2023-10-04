using Domain.Configurations;
using Microsoft.Azure.ServiceBus;
using ServiceBus.Producer.Enumeration;

namespace ServiceBus.Consumer.Extensions;

public static class SubscriptionClientFactoryExtensions
{
    public static SubscriptionClient CreateSubscriptionClient(this AzureServiceConfiguration configuration, EventsNamesEnums value) =>
        new (configuration.ServiceBusConnectionString, configuration.TopicName, value.Name);
}