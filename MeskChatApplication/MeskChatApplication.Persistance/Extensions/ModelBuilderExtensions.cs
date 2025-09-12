using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MeskChatApplication.Persistance.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplySoftDeleteFilter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Domain.Abstractions.Entity).IsAssignableFrom(entityType.ClrType) &&
                !entityType.ClrType.IsAbstract)
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var isDeletedProperty = Expression.Property(parameter, nameof(Domain.Abstractions.Entity.IsDeleted));
                var filter = Expression.Lambda(Expression.Equal(isDeletedProperty, Expression.Constant(false)), parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}