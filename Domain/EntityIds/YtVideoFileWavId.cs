using Domain.EntityIds.Base;

namespace Domain.EntityIds;

public record YtVideoFileWavId : EntityId
{
    public YtVideoFileWavId() : base()
    {
    }

    public YtVideoFileWavId(string ulid) : base(ulid)
    {
    }
    
    public override string ToString() => Value.ToString();
}