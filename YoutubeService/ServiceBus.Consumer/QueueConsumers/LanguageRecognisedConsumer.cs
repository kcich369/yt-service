using System.Text.Json;
using Domain.EntityIds;
using Domain.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Consumer.QueueConsumers.Base;
using ServiceBus.Producer.Enumeration;
using ServiceBus.Producer.Messages;

namespace ServiceBus.Consumer.QueueConsumers;

public sealed class LanguageRecognisedConsumer : QueueConsumerBackgroundService
{
    public LanguageRecognisedConsumer(IServiceProvider serviceProvider) : base(serviceProvider,
        EventsNamesEnums.LanguageRecognised)
    {
    }

    protected override async Task Execute(IServiceProvider serviceProvider, string message, CancellationToken token)
    {
        using var scope = serviceProvider.CreateScope();
        var languageRecognised = JsonSerializer.Deserialize<LanguageRecognised>(message);
        BackgroundJob.Enqueue<ITranscribeWavFileService>((service) =>
            service.Transcribe(new YtVideoFileWavId(languageRecognised.YtVideoFileWav), token));
    }
}