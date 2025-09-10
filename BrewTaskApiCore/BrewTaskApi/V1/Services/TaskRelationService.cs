using BrewTaskApi.Database.Contexts;
using BrewTaskApi.Database.Entities;
using BrewTaskApi.Database.Services;
using BrewTaskApi.V1.Services.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BrewTaskApi.V1.Services
{

    /// <summary>
    /// task relation
    /// </summary>
    /// <param name="context"></param>
    [BusinessService]
    public class TaskRelationService(BrewTaskContext context) : SoftDeletedService<TaskRelation>(context)
    {

        /// <summary>
        /// get all relation on task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task<TaskRelationResponse[]> GetRelationsAsync(int taskId)
            => context
            .TaskRelations
            .AsNoTracking()
            .Where(w => w.TaskFromId == taskId || w.TaskToId == taskId)
            .Select(s => new TaskRelationResponse(s.Id, s.TaskFromId, s.TaskToId, (int)s.RelationType, s.CreateAt))
            .ToArrayAsync();

        /// <summary>
        /// create
        /// </summary>
        /// <param name="taskFromId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TaskRelationResponse?> CreateAsync(int taskFromId, TaskRelationCreateRequest request)
        {
            var requestType = (RelationTypeTask)request.RelationType;
            if (await context.TaskRelations
                .AnyAsync(a => a.TaskFromId == taskFromId
                        && a.TaskToId == request.TaskToId
                        && a.RelationType == requestType))
                return null;

            if(requestType == RelationTypeTask.Blocks)
            {
                if(await context.TaskRelations
                    .AnyAsync(a => a.TaskFromId == request.TaskToId && a.TaskToId == taskFromId))
                    return null;
            }


            var operation = await context
                .TaskRelations
                .AddAsync(new()
                {
                    RelationType = requestType,
                    TaskFromId = taskFromId,
                    TaskToId = request.TaskToId
                });
            await context.SaveChangesAsync();

            var entity = operation.Entity;
            return new(entity.Id, entity.TaskFromId, entity.TaskToId, request.RelationType, entity.CreateAt);
        }

        /// <summary>
        /// task relation
        /// </summary>
        /// <param name="TaskToId"></param>
        /// <param name="RelationType"></param>
        public record TaskRelationCreateRequest(int? TaskToId, int RelationType);

        /// <summary>
        /// task relation
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="TaskFromId"></param>
        /// <param name="TaskToId"></param>
        /// <param name="RelationType"></param>
        /// <param name="CreateAt"></param>
        public record TaskRelationResponse(int Id, int? TaskFromId, int? TaskToId, int RelationType, DateTime CreateAt);
    }
}
