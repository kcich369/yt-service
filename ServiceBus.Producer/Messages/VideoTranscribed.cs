using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class VideoTranscribed : MessageBase<YtVideoTranscriptionId>
{
    public VideoTranscribed(YtVideoTranscriptionId entityId) : base(entityId)
    {
    }
}