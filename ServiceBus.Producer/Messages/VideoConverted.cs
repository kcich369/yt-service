using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoConverted : MessageBase<YtVideoFileWavId>
{
    public VideoConverted(YtVideoFileWavId entityId) : base(entityId)
    {
    }
}
