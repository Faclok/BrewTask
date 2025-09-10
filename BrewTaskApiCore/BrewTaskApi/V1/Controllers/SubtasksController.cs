using Asp.Versioning;
using BrewTaskApi.V1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrewTaskApi.V1.Controllers
{

    /// <summary>
    /// subtasks
    /// </summary>
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubtasksController : ControllerBase
    {

        /// <summary>
        /// update title
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update(int id, string title, [FromServices] SubtasksService service)
        {
            if (await service.UpdateAsync(id, title) > 0)
                return NoContent();

            return NotFound();
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
        public async Task<IActionResult> Delete(int id, [FromServices] SubtasksService service)
        {
            if (await service.SoftDeleteAsync(w => w.Id == id) > 0)
                return NoContent();

            return NotFound();
        }
    }
}
