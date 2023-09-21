using ServiceBus.Producer.Messages.Base;

namespace ServiceBus.Producer.Publisher;

public interface IMessagePublisher
{
    Task Send<T>(T message) where T : IMessage;
}