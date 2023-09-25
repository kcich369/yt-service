using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class ChannelCreated : MessageBase<YtChannelId>
{
    public ChannelCreated(YtChannelId ytChannelId) : base(ytChannelId)
    {
    }
}