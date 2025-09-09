using Asp.Versioning;
using BrewTaskApi.V1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// this is endpoint only use on test
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<JwtResponse>(StatusCodes.Status200OK)]
        public IActionResult GetToken([FromServices] JwtService service)
        {
            var login = Guid.NewGuid().ToString();
            var id = Random.Shared.Next();

            return Ok(new JwtResponse(service.GenerateJwtToken(login, id)));
        }

        /// <summary>
        /// token
        /// </summary>
        /// <param name="Token"></param>
        public record JwtResponse(string Token);
    }
}
