using BrewTaskApi.Database.Contexts;
using BrewTaskApi.Database.Entities;
using BrewTaskApi.Database.Services;
using BrewTaskApi.V1.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskEntity = BrewTaskApi.Database.Entities.Task;

namespace BrewTaskApi.V1.Services
{

    /// <summary>
    /// tasks
    /// </summary>
    /// <param name="context"></param>
    [BusinessService]
    public class TasksService(BrewTaskContext context): SoftDeletedService<TaskEntity>(context)
    {

        /// <summary>
        /// get task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<TaskResponse?> GetAsync(int id)
            => context
            .Tasks
            .AsNoTracking()
            .Where(w => w.Id == id)
            .Select(s => new TaskResponse(
                s.Id, 
                s.Title, 
                s.Description,
                (int) s.Status,
                (int) s.Priority,
                s.AuthorId,
                s.AssigneeId,
                s.CreateAt,
                s.UpdateAt,
                s.RelationTasksTo.Count(),
                s.RelationTasksFrom.Count(),
                s.Subtasks.Count()))
            .FirstOrDefaultAsync();

        /// <summary>
        /// get task
        /// </summary>
        /// <returns></returns>
        public Task<TaskResponse[]> GetPageOnAuthorAsync(int userId, int pageSize, int pageNumber)
            => GetTaskOnPageAsync(t => t.AuthorId == userId, pageSize, pageNumber);

        /// <summary>
        /// get task
        /// </summary>
        /// <returns></returns>
        public Task<TaskResponse[]> GetPageOnAssigneeAsync(int userId, int pageSize, int pageNumber)
            => GetTaskOnPageAsync(t => t.AssigneeId == userId, pageSize, pageNumber);

        /// <summary>
        /// update task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(int id, TaskUpdateRequest request)
        {
            if (request.AssigneeId != null && await context.Users.AnyAsync(a => a.Id == request.AssigneeId))
                return false;

            return await context
            .Tasks
            .Where(w => w.Id == id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(p => p.Title, request.Title)
                .SetProperty(p => p.Description, request.Description)
                .SetProperty(p => p.Status, (StatusTask)request.Status)
                .SetProperty(p => p.Priority, (PriorityTask)request.Priority)
                .SetProperty(p => p.AssigneeId, request.AssigneeId)
                .SetProperty(p => p.UpdateAt, DateTime.UtcNow)) > 0;
        }

        /// <summary>
        /// create task
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TaskResponse?> CreateAsync(int userId, TaskCreateRequest request)
        {
            if (request.AssigneeId != null && await context.Users.AnyAsync(a => a.Id == request.AssigneeId))
                return null;

            if (await context.Users.AnyAsync(a => a.Id == userId))
                return null;

            var createOperation = await context
                .Tasks
                .AddAsync(new()
                {
                    AssigneeId = request.AssigneeId,
                    AuthorId = userId,
                    Description = request.Description,
                    Priority = (PriorityTask) request.Priority,
                    Status = (StatusTask) request.Status,
                    Title = request.Title
                });

            await context.SaveChangesAsync();

            var entity = createOperation.Entity;
            return new(entity.Id, entity.Title, entity.Description, 
                request.Status, request.Priority, entity.AuthorId, entity.AssigneeId,
                entity.CreateAt, entity.UpdateAt, 0, 0, 0); // zero is now create entity
        }

        private Task<TaskResponse[]> GetTaskOnPageAsync(Expression<Func<TaskEntity, bool>> predicate, int pageSize, int pageNumber)
            => context
            .Tasks
            .AsNoTracking()
            .AsSplitQuery()
            .Where(predicate)
            .OrderBy(b => b.Id)
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .Select(s => new TaskResponse(
                s.Id,
                s.Title,
                s.Description,
                (int)s.Status,
                (int)s.Priority,
                s.AuthorId,
                s.AssigneeId,
                s.CreateAt,
                s.UpdateAt,
                s.RelationTasksTo.Count(),
                s.RelationTasksFrom.Count(),
                s.Subtasks.Count()))
            .ToArrayAsync();


        /// <summary>
        /// task update
        /// </summary>
        public record TaskUpdateRequest(string Title, string Description,
            int Status, int Priority, int? AssigneeId);

        /// <summary>
        /// task create
        /// </summary>
        public record TaskCreateRequest(string Title, string Description,
            int Status, int Priority, int? AssigneeId);

        /// <summary>
        /// task response
        /// </summary>
        public record TaskResponse(int Id, string Title, string Description, 
            int Status, int Priority, int? AuthorId, int? AssigneeId, 
            DateTime CreateAt, DateTime UpdateAt, int RelationTasksToCount,
            int RelationTasksFromCount, int SubtasksCount);
    }

}
