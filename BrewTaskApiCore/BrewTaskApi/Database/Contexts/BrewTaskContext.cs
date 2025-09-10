using BrewTaskApi.Database.Entities;
using BrewTaskApi.Database.Entities.Abstractions;
using BrewTaskApi.Database.Extensions;
using BrewTaskApi.Database.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskEntity = BrewTaskApi.Database.Entities.Task;

namespace BrewTaskApi.Database.Contexts
{

    /// <summary>
    /// main context
    /// </summary>
    /// <param name="options"></param>
    public class BrewTaskContext(DbContextOptions<BrewTaskContext> options, SecurePasswordService securePassword) : DbContext(options)
    {

        #region Tables

        /// <summary>
        /// users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// tasks
        /// </summary>
        public DbSet<TaskEntity> Tasks { get; set; }

        /// <summary>
        /// sub tasks
        /// </summary>
        public DbSet<Subtasks> Subtasks { get; set; }

        /// <summary>
        /// task relation
        /// </summary>
        public DbSet<TaskRelation> TaskRelations { get; set; }

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

            modelBuilder.Entity<User>()
                .HasMany(u => u.AuthorTasks)
                .WithOne(t => t.Author)
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AssigneeTasks)
                .WithOne(t => t.Assignee)
                .HasForeignKey(t => t.AssigneeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Subtasks>()
                .HasOne(st => st.ParentTask)      
                .WithMany(t => t.Subtasks)
                .HasForeignKey(st => st.ParentTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            modelBuilder.Entity<TaskRelation>()
                .HasOne(tr => tr.TaskFrom)        
                .WithMany(t => t.RelationTasksFrom) 
                .HasForeignKey(tr => tr.TaskFromId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<TaskRelation>()
                .HasOne(tr => tr.TaskTo)         
                .WithMany(t => t.RelationTasksTo)
                .HasForeignKey(tr => tr.TaskToId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<User>()
                .HasData([
                    new(){
                        Id = 1,
                        Username = "vinokurov",
                        Email = "vino_kurov@inbox.ru",
                        PasswordHash = securePassword.Hash("vino_kurov@inbox.ru"),
                    }
                    ]);
        }

        #endregion
    }
}
