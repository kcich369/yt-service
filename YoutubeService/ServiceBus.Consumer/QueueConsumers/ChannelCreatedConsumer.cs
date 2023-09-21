using System.Text.Json;
using Domain.EntityIds;
using Domain.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Consumer.QueueConsumers.Base;
using ServiceBus.Producer.Enumeration;
using ServiceBus.Producer.Messages;

namespace ServiceBus.Consumer.QueueConsumers;

public sealed class ChannelCreatedConsumer : QueueConsumerBackgroundService
{
    public ChannelCreatedConsumer(IServiceProvider serviceProvider) : base(serviceProvider,
        EventsNamesEnums.ChannelCreated)
    {
    }

    protected override async Task Execute(IServiceProvider serviceProvider, string message, CancellationToken token)
    {
        using var scope = serviceProvider.CreateScope();
        var channelCreated = JsonSerializer.Deserialize<ChannelCreated>(message);
        BackgroundJob.Enqueue<IAddChannelVideosService>((service) => service
            .ApplyNewVideos(new YtChannelId(channelCreated.ChannelId), token));
    }
}