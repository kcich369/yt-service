using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class NewVideoCreated : MessageBase<YtVideoId>
{
    public NewVideoCreated(YtVideoId entityId) : base(entityId)
    {
    }
}