using BrewTaskApi.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BrewTaskApi.Database.Contexts
{

    /// <summary>
    /// factory command
    /// </summary>
    public class BrewTaskContextFactory(SecurePasswordService securePassword) : IDesignTimeDbContextFactory<BrewTaskContext>
    {

        /// <summary>
        /// create context
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public BrewTaskContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BrewTaskContext>();

            // Важное: просто корректная строка, БД реально подключаться не будет при `migrations add`
            var connectionString = "Host=localhost;Port=5433;Database=usersdb;Username=postgres;Password=здесь_указывается_пароль_от_postgres";

            optionsBuilder.UseNpgsql(connectionString);

            return new BrewTaskContext(optionsBuilder.Options, securePassword);
        }
    }
}