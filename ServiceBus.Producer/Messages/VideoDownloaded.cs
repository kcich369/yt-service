using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoDownloaded : MessageBase<YtVideoFileId, YtVideoId>
{
    public VideoDownloaded(YtVideoFileId id, YtVideoId? previousId = null) : base(id, previousId)
    {
    }
}