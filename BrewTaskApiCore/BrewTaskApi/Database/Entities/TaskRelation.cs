using BrewTaskApi.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTaskApi.Database.Entities
{

    /// <summary>
    /// Task Relation
    /// </summary>
    public class TaskRelation: BaseEntityIdSoftDeleted
    {


        /// <summary>
        /// from task
        /// </summary>
        public int? TaskFromId { get; set; }

        /// <summary>
        /// from task
        /// </summary>
        [ForeignKey(nameof(TaskFromId))]
        public Task? TaskFrom { get; set; }

        /// <summary>
        /// task to
        /// </summary>
        public int? TaskToId { get; set; }

        /// <summary>
        /// task to
        /// </summary>
        [ForeignKey(nameof(TaskToId))]
        public Task? TaskTo { get; set; }

        /// <summary>
        /// relation type
        /// </summary>
        public RelationTypeTask RelationType { get; set; }

        /// <summary>
        /// create at
        /// </summary>
        public DateTime CreateAt { get; private set; } = DateTime.UtcNow;
    }
}
