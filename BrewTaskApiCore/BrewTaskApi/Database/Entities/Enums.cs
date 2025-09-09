namespace BrewTaskApi.Database.Entities
{

    /// <summary>
    /// relate type
    /// </summary>
    public enum RelationTypeTask
    {

        /// <summary>
        /// none
        /// </summary>
        None = 0,

        /// <summary>
        /// relate
        /// </summary>
        Related = 1,

        /// <summary>
        /// blocks
        /// </summary>
        Blocks = 2,

        /// <summary>
        /// dup
        /// </summary>
        Duplicates = 3
    }

    /// <summary>
    /// Priority Task
    /// </summary>
    public enum PriorityTask
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0,

        /// <summary>
        /// low
        /// </summary>
        Low = 1,

        /// <summary>
        /// medium
        /// </summary>
        Medium = 2,

        /// <summary>
        /// high
        /// </summary>
        High = 3
    }


    /// <summary>
    /// status task
    /// </summary>
    public enum StatusTask
    {
        /// <summary>
        /// none
        /// </summary>
        None = 0,

        /// <summary>
        /// new
        /// </summary>
        New = 1,

        /// <summary>
        /// progress
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// done
        /// </summary>
        Done = 3,
    }
}
