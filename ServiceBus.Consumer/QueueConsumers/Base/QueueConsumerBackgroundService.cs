using System.Reflection;
using System.Text;
using Domain.Configurations;
using Domain.EntityIds;
using Domain.EntityIds.Base;
using Domain.Helpers;
using Domain.Messages;
using Domain.Results;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceBus.Consumer.Extensions;
using ServiceBus.Producer.Enumeration;

namespace ServiceBus.Consumer.QueueConsumers.Base;

public abstract class QueueConsumerBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly EventsNamesEnums _eventsNamesEnums;
    private ILogger<QueueConsumerBackgroundService> _logger;
    private IMessageHelper _messageHelper;

    protected QueueConsumerBackgroundService(IServiceProvider serviceProvider,
        EventsNamesEnums eventsNamesEnums)
    {
        _serviceProvider = serviceProvider;
        _eventsNamesEnums = eventsNamesEnums;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var subscriptionClient = scope.ServiceProvider
            .GetRequiredService<AzureServiceConfiguration>()
            .CreateSubscriptionClient(_eventsNamesEnums);
        GetRequiredServices();
        subscriptionClient.RegisterMessageHandler(
            async (msg, token) =>
            {
                var message = Encoding.UTF8.GetString(msg.Body);
                _logger.LogInformation("Received message queue, name: {Name}, body:{Msg}", _eventsNamesEnums.Name,
                    message);
                await Execute(message, token);
                await subscriptionClient.CompleteAsync(msg.SystemProperties.LockToken);
            },
            new MessageHandlerOptions(args => Task.CompletedTask) { AutoComplete = false, MaxConcurrentCalls = 1 });
    }

    protected abstract Task Execute(string message, CancellationToken token);

    private void GetRequiredServices()
    {
        using var scope = _serviceProvider.CreateScope();
        _messageHelper = scope.ServiceProvider.GetRequiredService<IMessageHelper>();
        _logger = scope.ServiceProvider.GetRequiredService<ILogger<QueueConsumerBackgroundService>>();
    }

    protected async Task<IResult<bool>> HandleMessage<TId, TPreviousId>(MessageBase<TId, TPreviousId> message)
        where TId : EntityId where TPreviousId : EntityId
    {
        LogErrorMessageResult(await _messageHelper.PreviousOperationIsFinished(message)
            .TryCatch());
        var deliveringResult = await _messageHelper.Delivered(message).TryCatch();
        LogErrorMessageResult(deliveringResult);
        return deliveringResult;
    }

    private IResult LogErrorMessageResult(IResult result)
    {
        if (!result.IsError) return result;
        _logger.LogError("Error occured in queue consumer: {Name}{Consumer}, error message: {ErrorMessage}",
            _eventsNamesEnums.Name, "Consumer", result.ErrorMessage);
        return result;
    }
}