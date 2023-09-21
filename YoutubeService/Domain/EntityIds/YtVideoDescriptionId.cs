using Domain.EntityIds.Base;

namespace Domain.EntityIds;

public record YtVideoDescriptionId : EntityId
{
    public YtVideoDescriptionId():base()
    {
    }

    public YtVideoDescriptionId(string ulid) : base(ulid)
    {
    }
    public override string ToString() => Value.ToString();
}