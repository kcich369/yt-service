using Domain.EntityIds;

namespace Domain.Auditable;

public interface IUpdated
{
    DateTimeOffset? UpdatedAt { get; }
    UserId UpdatedById { get; }
    string UpdatedBy { get; }
    void SetUpdatedData(DateTimeOffset updatedAt, UserId updatedById, string updatedBy);
}