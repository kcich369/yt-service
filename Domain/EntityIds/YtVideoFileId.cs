using Domain.EntityIds.Base;
using Newtonsoft.Json;

namespace Domain.EntityIds;

public record YtVideoFileId : EntityId
{
    public YtVideoFileId():base()
    {
    }
    public YtVideoFileId(string ulid): base(ulid)
    {
    }
    public override string ToString() => Value.ToString();
}