using System.Text.Json;
using Domain.EntityIds;
using Domain.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Consumer.QueueConsumers.Base;
using ServiceBus.Producer.Enumeration;
using ServiceBus.Producer.Messages;

namespace ServiceBus.Consumer.QueueConsumers;

public sealed class VideoDownloadedConsumer : QueueConsumerBackgroundService
{
    public VideoDownloadedConsumer(IServiceProvider serviceProvider)
        : base(serviceProvider, EventsNamesEnums.VideoDownloaded)
    {
    }

    protected override async Task Execute(IServiceProvider serviceProvider, string message, CancellationToken token)
    {
        using var scope = serviceProvider.CreateScope();
        var videoDownloaded = JsonSerializer.Deserialize<VideoDownloaded>(message);
        BackgroundJob.Enqueue<IConvertVideoFileToWavService>((service) =>
            service.Convert(new YtVideoFileId(videoDownloaded.YtVideoFileId), token));
    }
}