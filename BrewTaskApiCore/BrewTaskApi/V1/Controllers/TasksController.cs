using Asp.Versioning;
using BrewTaskApi.JWT;
using BrewTaskApi.V1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrewTaskApi.V1.Controllers
{

    /// <summary>
    /// tasks
    /// </summary>
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TasksController : ControllerBase
    {

        /// <summary>
        /// get task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<TasksService.TaskResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id, [FromServices] TasksService service)
        {
            if (await service.GetAsync(id) is not { } task)
                return NotFound();

            return Ok(task);
        }

        /// <summary>
        /// get tasks
        /// </summary>
        /// <param name="userType">use assignee or author, if not, then empty array</param>
        /// <param name="service"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("{userType}/me/pages/{pageSize}/{pageNumber}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<TasksService.TaskResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string userType, int pageSize, int pageNumber, [FromServices] TasksService service)
        {
            if (HttpContext.GetClientId() is not { } userId)
                return Unauthorized();

            var tasks = userType switch
            {
                "assignee" => service.GetPageOnAssigneeAsync(userId, pageSize, pageNumber),
                "author" => service.GetPageOnAuthorAsync(userId, pageSize, pageNumber),
                _ => Task.FromResult(Array.Empty<TasksService.TaskResponse>())
            };

            return Ok(await tasks);
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, [FromBody] TasksService.TaskUpdateRequest request, [FromServices] TasksService service)
        {
            if (await service.UpdateAsync(id, request))
                return NoContent();

            return NotFound();
        }


        /// <summary>
        /// update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, int status, [FromServices] TasksService service)
        {
            if (await service.UpdateStatusAsync(id, status))
                return NoContent();

            return BadRequest();
        }


        /// <summary>
        /// create
        /// </summary>
        /// <param name="request"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType<TasksService.TaskResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TasksService.TaskCreateRequest request, [FromServices] TasksService service)
        {
            if (HttpContext.GetClientId() is not { } userId)
                return Unauthorized();

            if (await service.CreateAsync(userId, request) is not { } task)
                return NotFound();

            return Ok(task);
        }

        /// <summary>
        /// soft delete
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id, [FromServices] TasksService service)
        {
            if (HttpContext.GetClientId() is not { } userId)
                return Unauthorized();

            if (await service.SoftDeleteAsync(w => w.AuthorId == userId && w.Id == id) > 0)
                return NoContent();

            return NotFound();
        }

        #region subtasks

        /// <summary>
        /// get subtasks
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpGet("{id}/subtasks")]
        [ProducesResponseType<SubtasksService.SubtaskResponse[]>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSubtasks(int id, [FromServices] SubtasksService service)
            => Ok(await service.GetOnTaskAsync(id));

        /// <summary>
        /// create subtasks
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost("{id}/subtasks")]
        [ProducesResponseType<SubtasksService.SubtaskResponse[]>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateSubtasks(int id, [FromBody] SubtasksService.SubtaskCreateRequest request, [FromServices] SubtasksService service)
        {
            if (await service.CreateAsync(id, request) is not { } subtask)
                return NotFound();

            return Ok(subtask);
        }

        #endregion

        #region relation

        /// <summary>
        /// get relations
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpGet("{id}/relations")]
        [ProducesResponseType<TaskRelationService.TaskRelationResponse[]>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRelation(int id, [FromServices] TaskRelationService service)
            => Ok(await service.GetRelationsAsync(id));

        /// <summary>
        /// create relations
        /// </summary>
        /// <param name="id"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPost("{id}/relations")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType<TaskRelationService.TaskRelationResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateRelation(int id, [FromBody] TaskRelationService.TaskRelationCreateRequest request, [FromServices] TaskRelationService service)
        {
            if (await service.CreateAsync(id, request) is not { } relation)
                return BadRequest();

            return Ok(relation);
        }

        #endregion
    }
}
