using Domain.EntityIds;
using Domain.Messages;

namespace ServiceBus.Producer.Messages;

public sealed class LanguageRecognised : MessageBase<YtVideoFileWavId>
{
    public LanguageRecognised(YtVideoFileWavId entityId) : base(entityId)
    {
    }
}
