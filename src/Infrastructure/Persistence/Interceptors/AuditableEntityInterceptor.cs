namespace Argo.CA.Infrastructure.Persistence.Interceptors;

using Argo.CA.Application.Common.Security.CurrentUserProvider;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class AuditableEntityInterceptor(
    ICurrentUserProvider currentUserProvider,
    TimeProvider dateTime)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;
        var currentUser = currentUserProvider.GetCurrentUser();

        foreach (var entry in context.ChangeTracker
                     .Entries<IAuditCreated>()
                     .Where(e => e.State == EntityState.Added)
                     .Select(e => e.Entity))
        {
            entry.SetCreated(dateTime.GetUtcNow(), currentUser.UserName);
        }

        foreach (var entry in context.ChangeTracker
                     .Entries<IAuditModified>()
                     .Where(e => e.HasChanges())
                     .Select(e => e.Entity))
        {
            entry.SetModified(dateTime.GetUtcNow(), currentUser.UserName);
        }
    }
}

public static class EntityEntryExtensions
{
    public static bool HasChanges(this EntityEntry entry) =>
        entry.State == EntityState.Added
        || entry.State == EntityState.Modified
        || entry.HasChangedOwnedEntities();

    private static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
