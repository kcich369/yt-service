using System.Text;
using Domain.Configurations;
using Domain.EntityIds;
using Domain.Services;
using Hangfire;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceBus.Consumer.Extensions;
using ServiceBus.Producer.Enumeration;

namespace ServiceBus.Consumer.QueueConsumers.Base;

public abstract class QueueConsumerBackgroundService : BackgroundService
{
    private readonly EventsNamesEnums _eventsNamesEnums;
    private IServiceProvider ServiceProvider { get; }

    protected QueueConsumerBackgroundService(IServiceProvider serviceProvider,
        EventsNamesEnums eventsNamesEnums)
    {
        _eventsNamesEnums = eventsNamesEnums;
        ServiceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var subscriptionClient = ServiceProvider.CreateScope().ServiceProvider
            .GetRequiredService<AzureServiceBusConfiguration>()
            .CreateSubscriptionClient(_eventsNamesEnums);
        subscriptionClient.RegisterMessageHandler(
            async (msg, token) =>
            {
                await MessageHandler(ServiceProvider, msg, token);
                await subscriptionClient.CompleteAsync(msg.SystemProperties.LockToken);
            },
            new MessageHandlerOptions(args => Task.CompletedTask) { AutoComplete = false, MaxConcurrentCalls = 1 });
    }

    private async Task MessageHandler(IServiceProvider serviceProvider, Message message, CancellationToken token)=>
        await Execute(serviceProvider, Encoding.UTF8.GetString(message.Body), token);

    protected abstract Task Execute(IServiceProvider serviceProvider, string message, CancellationToken token);
}