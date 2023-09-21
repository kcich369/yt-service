using Domain.EntityIds.Base;

namespace Domain.EntityIds;

public record YtVideoTranscriptionId : EntityId
{
    public YtVideoTranscriptionId() : base()
    {
    }

    public YtVideoTranscriptionId(string ulid) : base(ulid)
    {
    }
    
    public override string ToString() => Value.ToString();
}