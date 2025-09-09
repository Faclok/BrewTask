namespace BrewTaskApi.Database.Entities.Abstractions
{

    /// <summary>
    /// Мягкое удаление
    /// </summary>
    public interface ISoftDeleted
    {
        /// <summary>
        /// Удален ли он
        /// </summary>
        public bool SoftDeleted { get; set; }
    }
}
