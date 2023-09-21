using Domain.Auditable;
using Domain.EntityIds;
using Domain.Providers;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Persistence.Interceptors;

public sealed class CreatingAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IDateProvider _provider;

    public CreatingAuditableEntitiesInterceptor(IDateProvider provider)
    {
        _provider = provider;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        var entries = context.ChangeTracker.Entries<ICreated>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreationData(_provider.DateTimeNow(), UserId.GetUserId(), "admin");
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}