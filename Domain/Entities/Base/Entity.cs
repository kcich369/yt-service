using Domain.EntityIds.Base;

namespace Domain.Entities.Base;

public abstract class Entity<TId> : IEntity where TId : EntityId
{
    public TId Id { get; protected init; }

    public bool Deleted { get; protected set; }
    public byte[] Version { get; protected set; }

    protected Entity()
    {
    }

    public void Delete()
    {
        Deleted = true;
    }
}