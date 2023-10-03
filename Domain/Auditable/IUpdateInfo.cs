using Domain.EntityIds;
using Domain.ValueObjects;

namespace Domain.Auditable;

public interface IUpdateInfo
{
    UpdateInfo UpdateInfo { get; }
    void SetUpdateInfoData(DateTimeOffset updatedAt, UserId updatedById, string updatedBy);
}