using BrewTaskApi.Database.Contexts;
using BrewTaskApi.Database.Entities;
using BrewTaskApi.Database.Services;
using BrewTaskApi.V1.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BrewTaskApi.V1.Services
{

    /// <summary>
    /// subtasks
    /// </summary>
    /// <param name="context"></param>
    [BusinessService]
    public class SubtasksService(BrewTaskContext context): SoftDeletedService<Subtasks>(context)
    {

        /// <summary>
        /// tasks on task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task<SubtaskResponse[]> GetOnTaskAsync(int taskId)
            => context
            .Subtasks
            .AsNoTracking()
            .Where(w => w.ParentTaskId == taskId)
            .Select(s => new SubtaskResponse(s.Id, s.Title, s.TaskId))
            .ToArrayAsync();

        /// <summary>
        /// update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(int id, string title)
            => context
            .Subtasks
            .Where(w => w.Id == id)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.Title, title));

        /// <summary>
        /// create
        /// </summary>
        /// <param name="parentTaskId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SubtaskResponse?> CreateAsync(int parentTaskId, SubtaskCreateRequest request)
        {
            if (!await context.Tasks.AnyAsync(a => a.Id == parentTaskId))
                return null;

            if (!await context.Tasks.AnyAsync(a => a.Id == request.TaskId))
                return null;

            var operation = await context
                .Subtasks
                .AddAsync(new()
                {
                    ParentTaskId = parentTaskId,
                    TaskId = request.TaskId,
                    Title = request.Title
                });

            await context.SaveChangesAsync();
            var entity = operation.Entity;

            return new(entity.Id, entity.Title, entity.TaskId);
        }

        /// <summary>
        /// subtask
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="TaskId"></param>
        public record SubtaskCreateRequest(string Title, int TaskId);


        /// <summary>
        /// subtask
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Title"></param>
        /// <param name="TaskId"></param>
        public record SubtaskResponse(int Id, string Title, int? TaskId);
    }
}
