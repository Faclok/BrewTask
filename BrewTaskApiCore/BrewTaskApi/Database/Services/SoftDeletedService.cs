using BrewTaskApi.Database.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BrewTaskApi.Database.Services
{

    /// <summary>
    /// soft deleted
    /// </summary>
    public abstract class SoftDeletedService<T>(DbContext context) where T: class, ISoftDeleted
    {

        /// <summary>
        /// soft delete
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<int> SoftDeleteAsync(Expression<Func<T, bool>> predicate)
            => context
            .Set<T>()
            .Where(predicate)
            .ExecuteUpdateAsync(s => 
                s.SetProperty(p => p.SoftDeleted, true));
    }
}
