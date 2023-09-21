using System.Text.Json;
using Domain.EntityIds;
using Domain.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using ServiceBus.Consumer.QueueConsumers.Base;
using ServiceBus.Producer.Enumeration;
using ServiceBus.Producer.Messages;

namespace ServiceBus.Consumer.QueueConsumers;

public sealed class NewVideoCreatedConsumer : QueueConsumerBackgroundService
{
    public NewVideoCreatedConsumer(IServiceProvider serviceProvider)
        : base(serviceProvider, EventsNamesEnums.NewVideoCreated)
    {
    }

    protected override async Task Execute(IServiceProvider serviceProvider, string message, CancellationToken token)
    {
        using var scope = serviceProvider.CreateScope();
        var newVideoCreated = JsonSerializer.Deserialize<NewVideoCreated>(message);
        BackgroundJob.Enqueue<IDownloadYtVideoFilesService>(service => 
            service.Download(new YtVideoId(newVideoCreated.YtVideoId), token));
    }
}