using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoDownloaded : MessageBase<YtVideoFileId>
{
    public VideoDownloaded(YtVideoFileId entityId) : base(entityId)
    {
    }
}