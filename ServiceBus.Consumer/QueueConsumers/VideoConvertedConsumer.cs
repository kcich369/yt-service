using System.Text.Json;
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

public sealed class VideoConvertedConsumer : QueueConsumerBackgroundService
{
    public VideoConvertedConsumer(IServiceProvider serviceProvider)
        : base(serviceProvider, EventsNamesEnums.VideoConverted)
    {
    }

    protected override async Task Execute(string message, CancellationToken token)
    {
        var newVideoCreated = JsonSerializer.Deserialize<VideoConverted>(message);
        if ((await HandleMessage(newVideoCreated)).Data)
            return;
        BackgroundJob.Enqueue<IRecogniseLanguageService>(HangfireQueuesEnum.RecognisingLanguage.Name, (service) =>
            service.Recognise(newVideoCreated.Id, token));
    }
}