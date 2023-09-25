using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoDataAdded : MessageBase<YtVideoId>
{
    public VideoDataAdded(YtVideoId entityId) : base(entityId)
    {
    }
}