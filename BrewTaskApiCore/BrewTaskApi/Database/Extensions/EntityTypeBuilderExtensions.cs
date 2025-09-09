using BrewTaskApi.Database.Entities.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace BrewTaskApi.Database.Extensions
{

    /// <summary>
    /// Поддержка типов
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {

        /// <summary>
        /// Установление фильтра
        /// </summary>
        /// <param name="entityType"></param>
        public static void SetQueryFilter(this IMutableEntityType entityType)
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");

            // Доступ к свойству SoftDeleted
            var property = Expression.Property(parameter, nameof(ISoftDeleted.SoftDeleted));

            // Создание выражения !e.SoftDeleted
            var filter = Expression.Lambda(
                Expression.Not(property),
                parameter
            );

            entityType.SetQueryFilter(filter);
        }
    }
}
