using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoConverted : MessageBase<YtVideoFileWavId, YtVideoFileId>
{
    public VideoConverted(YtVideoFileWavId id, YtVideoFileId? previousId = null) : base(id, previousId)
    {
    }
}