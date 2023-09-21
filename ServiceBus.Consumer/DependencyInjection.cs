using Domain.Configurations;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Consumer.QueueConsumers;

namespace ServiceBus.Consumer;

public static class DependencyInjection
{
    public static IServiceCollection RegisterServiceBusConsumer(this IServiceCollection serviceCollection,
        JobsConfiguration jobsConfiguration)
    {
        if (jobsConfiguration.DisableAllQueueConsumers)
            return serviceCollection;

        serviceCollection.AddHostedService<ChannelCreatedConsumer>();
        serviceCollection.AddHostedService<LanguageRecognisedConsumer>();
        serviceCollection.AddHostedService<NewVideoCreatedConsumer>();
        serviceCollection.AddHostedService<VideoConvertedConsumer>();
        serviceCollection.AddHostedService<VideoDataAddedConsumer>();
        serviceCollection.AddHostedService<VideoDownloadedConsumer>();
        serviceCollection.AddHostedService<VideoTranscribedConsumer>();

        return serviceCollection;
    }
}