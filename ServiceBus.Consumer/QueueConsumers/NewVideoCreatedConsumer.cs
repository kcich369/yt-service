using System.Text.Json;
using Domain.EntityIds;
using Domain.Enumerations;
using Domain.Helpers;
using Domain.Results;
using Domain.Services;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

    protected override async Task Execute(string message, CancellationToken token)
    {
        var newVideoCreated = JsonSerializer.Deserialize<NewVideoCreated>(message);
        if ((await HandleMessage(newVideoCreated)).Data)
            return;
        BackgroundJob.Enqueue<IDownloadYtVideoFilesService>(HangfireQueuesEnum.Downloading.Name, service =>
            service.Download(newVideoCreated.Id, token));
    }
}