using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class LanguageRecognised : MessageBase<YtVideoFileWavId, YtVideoFileWavId>
{
    public LanguageRecognised(YtVideoFileWavId id, YtVideoFileWavId? previousId = null) : base(id, previousId)
    {
    }
}