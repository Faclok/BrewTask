using BrewTaskApi.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTaskApi.Database.Entities
{

    /// <summary>
    /// sub task
    /// </summary>
    public class Subtasks: BaseEntityIdSoftDeleted
    {

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// parent task
        /// </summary>
        public int? ParentTaskId { get; set; }

        /// <summary>
        /// parent task
        /// </summary>
        [ForeignKey(nameof(ParentTaskId))]
        public Task? ParentTask { get; set; }

        /// <summary>
        /// task
        /// </summary>
        public int? TaskId { get; set; }

        /// <summary>
        /// task
        /// </summary>
        [ForeignKey(nameof(TaskId))]
        public Task? Task { get; set; }
    }
}
