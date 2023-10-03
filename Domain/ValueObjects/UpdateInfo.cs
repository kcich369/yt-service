using Domain.EntityIds;

namespace Domain.ValueObjects;

public class UpdateInfo
{
    public DateTimeOffset? UpdatedAt { get; private set; }
    public UserId UpdatedById { get; private set; }
    public string UpdatedBy { get; private set; }

    public UpdateInfo(DateTimeOffset? updatedAt, UserId updatedById, string updatedBy)
    {
        UpdatedAt = updatedAt;
        UpdatedById = updatedById;
        UpdatedBy = updatedBy;
    }
}