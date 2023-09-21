using Domain.EntityIds;
using Domain.EntityIds.Base;

namespace Domain.Abstractions;

public abstract class Entity<TId> : IEntity where TId : EntityId
{
    public TId Id { get; protected init; }

    public bool Deleted { get; protected set; }

    protected Entity()
    {
    }

    public void Delete()
    {
        Deleted = true;
    }
}