using ServiceBus.Producer.Messages.Base;
using ServiceBus.Producer.Publisher;

namespace Infrastructure.Services.Base;

public abstract class MessagePublisherService<T> where T : IMessage
{
    private readonly IMessagePublisher _messagePublisher;

    protected MessagePublisherService(IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
    }

    protected async Task Publish(T message)
    {
        if (message is null)
            return;
        await _messagePublisher.Send(message);
    }

    protected async Task Publish(IEnumerable<T> messages)
    {
        messages = messages.ToList();
        if(!messages.Any())
            return;
        foreach (var message in messages)
        {
            await _messagePublisher.Send(message);
        }
    }
}