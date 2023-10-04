using Domain.Auditable;
using Domain.EntityIds;
using Domain.EntityIds.Base;
using Domain.ValueObjects;

namespace Domain.Entities.Base;

public abstract class Entity<TId> : IEntity, ICreationInfo, IUpdateInfo where TId : EntityId
{
    public TId Id { get; protected init; }
    public CreationInfo CreationInfo { get; private set; }
    public UpdateInfo UpdateInfo { get; private set; }

    public bool Deleted { get; protected set; }
    public byte[] Version { get; protected set; }

    protected Entity()
    {
    }

    public void Delete()
    {
        Deleted = true;
    }


    public void SetCreationInfo(DateTimeOffset createdAt, UserId createdById, string createdBy)
    {
        CreationInfo = new CreationInfo(createdAt, createdById, createdBy);
    }


    public void SetUpdateInfoData(DateTimeOffset updatedAt, UserId updatedById, string updatedBy)
    {
        UpdateInfo = new UpdateInfo(updatedAt, updatedById, updatedBy);
    }
}