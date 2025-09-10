using Asp.Versioning;
using BrewTaskApi.V1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace BrewTaskApi.V1.Controllers
{

    /// <summary>
    /// auth
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {

        // need move to service, but not is now
        #region try log

        /// <summary>
        /// count try for email
        /// </summary>
        public static readonly ConcurrentDictionary<string, int> _tryLogInCount = [];

        /// <summary>
        /// max count try log in.
        /// </summary>
        public const int MAX_COUNT = 35;

        #endregion

        /// <summary>
        /// this is endpoint only use on test
        /// If you try fail more 35, then you block list (403)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType<UsersSecureService.UsersSecureResponse>(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogIn([FromBody] UsersSecureService.UserLoginRequest request, [FromServices] UsersSecureService service)
        {
            if (_tryLogInCount.TryGetValue(request.Email, out var currentTry) && currentTry >= MAX_COUNT)
                return Forbid();

            if(await service.TryLogInAsync(request) is not { } response)
            {
                _tryLogInCount.AddOrUpdate(request.Email, 1, (k, v) => v + 1); // up to one
                return NotFound();
            }

            _tryLogInCount.TryRemove(request.Email, out _);

            return Ok(response);
        }

        /// <summary>
        /// check token
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CheckToken()
            => NoContent();
    }
}
