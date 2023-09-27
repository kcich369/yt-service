using System.Text;
using Domain.Configurations;
using Domain.EntityIds;
using Domain.Helpers;
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
    protected IMessageHelper MessageHelper;

    protected QueueConsumerBackgroundService(IServiceProvider serviceProvider,
        EventsNamesEnums eventsNamesEnums)
    {
        _eventsNamesEnums = eventsNamesEnums;
        ServiceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = ServiceProvider.CreateScope();
        MessageHelper = scope.ServiceProvider.GetRequiredService<IMessageHelper>();
        var subscriptionClient = scope.ServiceProvider
            .GetRequiredService<AzureServiceBusConfiguration>()
            .CreateSubscriptionClient(_eventsNamesEnums);

        subscriptionClient.RegisterMessageHandler(
            async (msg, token) =>
            {
                await Execute(Encoding.UTF8.GetString(msg.Body), token);
                await subscriptionClient.CompleteAsync(msg.SystemProperties.LockToken);
            },
            new MessageHandlerOptions(args => Task.CompletedTask) { AutoComplete = false, MaxConcurrentCalls = 1 });
    }

    protected abstract Task Execute(string message, CancellationToken token);
}