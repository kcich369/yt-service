using System.Text.Json;
using Domain.EntityIds;
using Domain.Helpers;
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

    protected override async Task Execute(string message, CancellationToken token)
    {
        var channelCreated = JsonSerializer.Deserialize<ChannelCreated>(message);
        if ((await HandleMessage(channelCreated)).Data)
            return;
        BackgroundJob.Enqueue<IAddChannelVideosService>((service) => service
            .ApplyNewVideos(channelCreated.Id, token));
    }
}