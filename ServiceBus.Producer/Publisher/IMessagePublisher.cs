using Domain.Messages;

namespace ServiceBus.Producer.Publisher;

public interface IMessagePublisher
{
    Task Send<T>(T message) where T : IMessage;
    Task Send<T>(IEnumerable<T> messages) where T : IMessage;
}