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
    }
}
