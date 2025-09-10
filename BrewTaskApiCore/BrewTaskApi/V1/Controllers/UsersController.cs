using Asp.Versioning;
using BrewTaskApi.JWT;
using BrewTaskApi.V1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrewTaskApi.V1.Controllers
{

    /// <summary>
    /// users
    /// </summary>
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {

        /// <summary>
        /// get me
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<UsersService.UserResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMeAsync([FromServices] UsersService service)
        {
            if (HttpContext.GetClientId() is not { } userId)
                return Unauthorized();

            if (await service.GetAsync(userId) is not { } user)
                return NotFound();

            return Ok(user);
        }
    }
}
