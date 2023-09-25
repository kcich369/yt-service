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

public sealed class NewVideoCreatedConsumer : QueueConsumerBackgroundService
{
    public NewVideoCreatedConsumer(IServiceProvider serviceProvider)
        : base(serviceProvider, EventsNamesEnums.NewVideoCreated)
    {
    }

    protected override async Task Execute(IMessageHelper messageHelper, string message, CancellationToken token)
    {
        var newVideoCreated = JsonSerializer.Deserialize<NewVideoCreated>(message);
        if(await messageHelper.MessageIsProcessing(newVideoCreated))
            return;
        BackgroundJob.Enqueue<IDownloadYtVideoFilesService>(service =>
            service.Download(newVideoCreated.Id, token));
    }
}