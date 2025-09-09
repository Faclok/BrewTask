using BrewTaskApi.Database.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTaskApi.Database.Entities
{

    /// <summary>
    /// tasks
    /// </summary>
    public class Task: BaseEntityIdSoftDeleted
    {

        /// <summary>
        /// title
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Status
        /// </summary>
        public StatusTask Status { get; set; } = StatusTask.New;

        /// <summary>
        /// Priority
        /// </summary>
        public PriorityTask Priority { get; set; } = PriorityTask.None;

        /// <summary>
        /// who create
        /// </summary>
        public int? AuthorId { get; set; }

        /// <summary>
        /// author
        /// </summary>
        [ForeignKey(nameof(AuthorId))]
        public User? Author { get; set; }

        /// <summary>
        /// who assignee
        /// </summary>
        public int? AssigneeId { get; set; }

        /// <summary>
        /// who assignee
        /// </summary>
        [ForeignKey(nameof(AssigneeId))]
        public User? Assignee { get; set; }

        /// <summary>
        /// create at
        /// </summary>
        public DateTime CreateAt { get; private set; }

        /// <summary>
        /// update at
        /// </summary>
        public DateTime UpdateAt { get; set; }

        /// <summary>
        /// tasks relation
        /// </summary>
        public ICollection<TaskRelation> RelationTasksTo { get; set; } = [];

        /// <summary>
        /// tasks relation
        /// </summary>
        public ICollection<TaskRelation> RelationTasksFrom { get; set; } = [];

        /// <summary>
        /// sub tasks
        /// </summary>
        public ICollection<Subtasks> Subtasks { get; set; } = [];
     
    }
    
}
