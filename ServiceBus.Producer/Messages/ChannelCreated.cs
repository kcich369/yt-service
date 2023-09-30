using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class ChannelCreated : MessageBase<YtChannelId, YtChannelId>
{
    public ChannelCreated()
    {
    }

    public ChannelCreated(YtChannelId id) : base(id)
    {
    }
}