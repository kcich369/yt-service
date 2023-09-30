using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoDataAdded : MessageBase<YtVideoId, YtVideoTranscriptionId>
{
    public VideoDataAdded(YtVideoId id, YtVideoTranscriptionId? previousId = null) : base(id, previousId)
    {
    }
}