using Domain.Auditable;
using Domain.EntityIds;

namespace Domain.ValueObjects;

public class CreationInfo
{
    public DateTimeOffset CreatedAt { get; private set; }
    public UserId CreatedById { get; private set; }
    public string CreatedBy { get; private set; }

    public CreationInfo(DateTimeOffset createdAt, UserId createdById, string createdBy)
    {
        CreatedAt = createdAt;
        CreatedById = createdById;
        CreatedBy = createdBy;
    }
}