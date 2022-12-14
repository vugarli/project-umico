using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectUmico.Application.Common.Interfaces;
using ProjectUmico.Domain.Common;

namespace ProjectUmico.Infrastructure.Persistance.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public AuditableEntitySaveChangesInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext dbContext)
    {
        foreach (var entity in dbContext.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property(e => e.CreatedAt).CurrentValue = DateTime.Now;
                entity.Property(e => e.CreatedBy).CurrentValue = _currentUserService.UserId;
            }
            if (entity.State == EntityState.Added || entity.State == EntityState.Modified || entity.HasUpdatedOwnedTypes())
            {
                entity.Property(e => e.LastModified).CurrentValue = DateTime.Now;
                entity.Property(e => e.LastModifiedBy).CurrentValue = _currentUserService.UserId;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasUpdatedOwnedTypes(this EntityEntry<BaseAuditableEntity> entityEntry)
    {
        return entityEntry.References.Any(e =>
            e.TargetEntry != null && e.TargetEntry.Metadata.IsOwned() &&
            (e.TargetEntry.State == EntityState.Added || e.TargetEntry.State == EntityState.Modified));
    }
}