using System.Text.Json;
using Domain.Helpers;
using Domain.Services;
using Hangfire;
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

    protected override async Task Execute(IMessageHelper messageHelper, string message, CancellationToken token)
    {
        var videoTranscribed = JsonSerializer.Deserialize<VideoTranscribed>(message);
        BackgroundJob.Enqueue<ITranscriptionDataService>((service) =>
            service.Create(videoTranscribed.Id, token));
    }
}