using System.Text.Json;
using Domain.EntityIds;
using Domain.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Consumer.QueueConsumers.Base;
using ServiceBus.Producer.Enumeration;
using ServiceBus.Producer.Messages;

namespace ServiceBus.Consumer.QueueConsumers;

public sealed class VideoTranscribedConsumer : QueueConsumerBackgroundService
{
    public VideoTranscribedConsumer(IServiceProvider serviceProvider) : base(serviceProvider,
        EventsNamesEnums.VideoTranscribed)
    {
    }

    protected override async Task Execute(IServiceProvider serviceProvider, string message, CancellationToken token)
    {
        using var scope = serviceProvider.CreateScope();
        var videoTranscribed = JsonSerializer.Deserialize<VideoTranscribed>(message);
        BackgroundJob.Enqueue<ITranscriptionDataService>((service) =>
            service.Create(new YtVideoTranscriptionId(videoTranscribed.YtVideoTranscriptionId), token));
    }
}