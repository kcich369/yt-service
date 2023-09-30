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

public sealed class VideoDataAddedConsumer : QueueConsumerBackgroundService
{
    public VideoDataAddedConsumer(IServiceProvider serviceProvider)
        : base(serviceProvider, EventsNamesEnums.VideoDataAdded)
    {
    }

    protected override async Task Execute(string message, CancellationToken token)
    {
        var videoDataAdded = JsonSerializer.Deserialize<VideoDataAdded>(message);
        if ((await HandleMessage(videoDataAdded)).Data)
            return;
    }
}