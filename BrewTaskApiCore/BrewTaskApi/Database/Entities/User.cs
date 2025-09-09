using BrewTaskApi.Database.Entities.Abstractions;

namespace BrewTaskApi.Database.Entities
{

    /// <summary>
    /// users
    /// </summary>
    public class User: BaseEntityIdSoftDeleted
    {

        /// <summary>
        /// unique name
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// password hash
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;    

        /// <summary>
        /// create at
        /// </summary>
        public DateTime CreateAt { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Author Tasks
        /// </summary>
        public ICollection<Task> AuthorTasks { get; set; } = [];

        /// <summary>
        /// Assignee Tasks
        /// </summary>
        public ICollection<Task> AssigneeTasks { get; set; } = [];
    }
}
