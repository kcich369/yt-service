using Domain.EntityIds.Base;

namespace Domain.Messages;

public abstract class MessageBase<TId, TPreviousId> : IMessage where TId : EntityId where TPreviousId : EntityId
{
    [Newtonsoft.Json.JsonProperty, System.Text.Json.Serialization.JsonInclude]
    public TId Id { get; private set; }

    [Newtonsoft.Json.JsonProperty, System.Text.Json.Serialization.JsonInclude]
    public TPreviousId? PreviousId { get; private set; }

    protected MessageBase()
    {
    }

    protected MessageBase(TId id, TPreviousId? previousId = null)
    {
        Id = id;
        PreviousId = previousId;
    }
}