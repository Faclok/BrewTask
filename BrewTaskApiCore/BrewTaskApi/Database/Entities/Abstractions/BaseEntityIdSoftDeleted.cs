
namespace BrewTaskApi.Database.Entities.Abstractions
{

    /// <summary>
    /// Базовая модель для всех моделей ORM
    /// </summary>
    public abstract class BaseEntityIdSoftDeleted : BaseEntityId, ISoftDeleted
    {

        /// <summary>
        /// Мягкое удаление
        /// </summary>
        public bool SoftDeleted { get; set; }
    }
}
