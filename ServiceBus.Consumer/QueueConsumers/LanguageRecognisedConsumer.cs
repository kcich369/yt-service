﻿using System.Text.Json;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Helpers;
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

    protected override async Task Execute(string message, CancellationToken token)
    {
        var languageRecognised = JsonSerializer.Deserialize<LanguageRecognised>(message);
        if ((await HandleMessage(languageRecognised)).Data)
            return;
        BackgroundJob.Enqueue<ITranscribeWavFileService>(HangfireQueuesEnum.Transcribing.Name, (service) =>
            service.Transcribe(languageRecognised.Id, token));
    }
}