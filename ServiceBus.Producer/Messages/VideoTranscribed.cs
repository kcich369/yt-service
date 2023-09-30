using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoTranscribed : MessageBase<YtVideoTranscriptionId, YtVideoFileWavId>
{
    public VideoTranscribed(YtVideoTranscriptionId id, YtVideoFileWavId? previousId = null) : base(id, previousId)
    {
    }
}