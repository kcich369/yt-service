using Domain.EntityIds.Base;

namespace Domain.EntityIds;

public record UserId : EntityId
{
    public UserId() : base()
    {
    }

    public UserId(string ulid) : base(ulid)
    {
    }
    
    public override string ToString() => Value.ToString();
    public static UserId GetUserId() => new UserId("01H55W9451S9P4WT1C3TZF2JND");
}