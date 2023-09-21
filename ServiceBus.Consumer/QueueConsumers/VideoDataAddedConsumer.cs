using System.Text.Json;
using Domain.EntityIds;
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

    protected override async Task Execute(IServiceProvider serviceProvider, string message, CancellationToken token)
    {
        using var scope = serviceProvider.CreateScope();
        var newVideoCreated = JsonSerializer.Deserialize<VideoDataAdded>(message);
    }
}