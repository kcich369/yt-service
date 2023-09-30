using Domain.EntityIds.Base;

namespace Domain.EntityIds;

public record YtChannelId : EntityId
{
    public YtChannelId()
    {
    }

    public YtChannelId(string ulid) : base(ulid)
    {
    }
    public override string ToString() => Value.ToString();
}