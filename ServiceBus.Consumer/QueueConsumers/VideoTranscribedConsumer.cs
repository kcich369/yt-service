﻿using System.Text.Json;
using Domain.Enumerations;
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

    protected override async Task Execute(string message, CancellationToken token)
    {
        var videoTranscribed = JsonSerializer.Deserialize<VideoTranscribed>(message);
        if ((await HandleMessage(videoTranscribed)).Data)
            return;
        BackgroundJob.Enqueue<ITranscriptionDataService>(HangfireQueuesEnum.TranscriptionOperations.Name, (service) =>
            service.Create(videoTranscribed.Id, token));
    }
}