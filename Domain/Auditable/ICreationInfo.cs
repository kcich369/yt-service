using Domain.EntityIds;
using Domain.ValueObjects;

namespace Domain.Auditable;

public interface ICreationInfo
{
    CreationInfo CreationInfo { get; }

    void SetCreationInfo(DateTimeOffset createdAt, UserId createdById, string createdBy);
}