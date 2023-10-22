using Domain.Auditable;
using Domain.EntityIds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.Interceptors;

public sealed class UpdatingAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        var entries = context.ChangeTracker.Entries<IUpdateInfo>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.SetUpdateInfoData(DateTimeOffset.Now, UserId.GetUserId(), "admin");
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}