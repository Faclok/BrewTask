using BrewTaskApi.Database.Entities.Abstractions;
using BrewTaskApi.Database.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BrewTaskApi.Database.Contexts
{

    /// <summary>
    /// main context
    /// </summary>
    /// <param name="options"></param>
    public class BrewTaskContext(DbContextOptions<BrewTaskContext> options) : DbContext(options)
    {

        #region Tables

        #endregion

        #region Configuration

        /// <summary>
        /// Переопределение базовой логики, с учетом мягкого удаления
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v, v => v.ToUniversalTime());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeleted).IsAssignableFrom(entityType.ClrType))
                    entityType.SetQueryFilter();

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(dateTimeConverter);
                }
            }
        }

        #endregion
    }
}
