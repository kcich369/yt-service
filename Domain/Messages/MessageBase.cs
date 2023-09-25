using Domain.EntityIds.Base;

namespace Domain.Messages;

public abstract class MessageBase<T> : IMessage where T : EntityId
{
    public T Id { get; private set; }

    protected MessageBase(T entityId)
    {
        Id = entityId;
    }
}