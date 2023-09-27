using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class NewVideoCreated : MessageBase<YtVideoId, YtChannelId>
{
    public NewVideoCreated(YtVideoId id, YtChannelId? previousId = null) : base(id, previousId)
    {
    }
}