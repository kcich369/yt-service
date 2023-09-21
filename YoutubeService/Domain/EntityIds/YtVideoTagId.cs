using Domain.EntityIds.Base;

namespace Domain.EntityIds;

public record YtVideoTagId : EntityId
{
    public YtVideoTagId() : base()
    {
    }

    public YtVideoTagId(string ulid) : base(ulid)
    {
    }
    
    public override string ToString() => Value.ToString();
}