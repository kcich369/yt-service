using Domain.EntityIds.Base;

namespace Domain.EntityIds;

public record YtVideoId : EntityId
{
    public YtVideoId() : base()
    {
    }

    public YtVideoId(string ulid) : base(ulid)
    {
    }

    public override string ToString() => Value.ToString();
}