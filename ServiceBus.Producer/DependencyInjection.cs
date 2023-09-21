using Domain.Configurations;
using Microsoft.Azure.ServiceBus;
using ServiceBus.Producer.Publisher;

namespace ServiceBus.Producer;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServiceBusProducer(this IServiceCollection serviceCollection,
        AzureServiceBusConfiguration configuration)
    {
        serviceCollection.AddSingleton<ITopicClient>((sp) =>
            new TopicClient(configuration.ConnectionString, configuration.TopicName));
        serviceCollection.AddTransient<IMessagePublisher, MessagePublisher>();
        return serviceCollection;
    }
}