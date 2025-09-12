using MeskChatApplication.Domain.Abstractions;
using MeskChatApplication.Persistance.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MeskChatApplication.Persistance.Context;

public sealed class ApplicationDatabaseContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);
        modelBuilder.ApplySoftDeleteFilter();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var entries = ChangeTracker.Entries<Entity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property(p => p.UpdatedAt).CurrentValue = DateTime.UtcNow;
                var isDeletedProp = entry.Property(p => p.IsDeleted);
                if (isDeletedProp.IsModified && (bool)isDeletedProp.CurrentValue)
                {
                    entry.Property(p => p.DeletedAt).CurrentValue = DateTime.UtcNow;
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}