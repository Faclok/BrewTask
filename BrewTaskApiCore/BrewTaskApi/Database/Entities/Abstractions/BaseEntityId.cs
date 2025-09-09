using System.ComponentModel.DataAnnotations.Schema;

namespace BrewTaskApi.Database.Entities.Abstractions
{

    /// <summary>
    /// Базовая сущность
    /// </summary>
    public abstract class BaseEntityId
    {

        /// <summary>
        /// Уникальный ключ, EF сам устанавливает его как ключ, если в конечной сущности нет Key Atrribute
        /// </summary>
        [Column("id")]
        public int Id { get; set; }
    }
}
