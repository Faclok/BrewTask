using Microsoft.EntityFrameworkCore;

namespace BrewTaskApi.Database.Extensions
{

    /// <summary>
    /// Класс с расширениями для базы данных
    /// </summary>
    public static class ConfigurationExtensions
    {

        /// <summary>
        /// Применяет миграцию если она есть
        /// </summary>
        /// <param name="host">Хост для получения базы данных</param>
        /// <returns>Задача</returns>
        public static async Task MigrateDatabaseAsync<T>(this IHost host)
            where T : DbContext
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            using var context = services.GetRequiredService<T>();

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<IHost>>();
                logger.LogError(ex, "An error occurred while migrating the database.");

                throw;
            }
        }
    }
}
