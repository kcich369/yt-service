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

public sealed class VideoDownloadedConsumer : QueueConsumerBackgroundService
{
    public VideoDownloadedConsumer(IServiceProvider serviceProvider)
        : base(serviceProvider, EventsNamesEnums.VideoDownloaded)
    {
    }

    protected override async Task Execute(string message, CancellationToken token)
    {
        var videoDownloaded = JsonSerializer.Deserialize<VideoDownloaded>(message);
        if ((await HandleMessage(videoDownloaded)).Data)
            return;
        BackgroundJob.Enqueue<IConvertVideoFileToWavService>((service) =>
            service.Convert(videoDownloaded.Id, token));
    }
}