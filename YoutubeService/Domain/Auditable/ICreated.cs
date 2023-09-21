using Domain.EntityIds;

namespace Domain.Auditable;

public interface ICreated
{
    DateTimeOffset CreatedAt { get; }
    UserId CreatedById { get; }
    string CreatedBy { get; }

    void SetCreationData(DateTimeOffset createdAt, UserId createdById, string createdBy);
}