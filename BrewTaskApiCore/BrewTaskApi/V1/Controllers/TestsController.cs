using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrewTaskApi.V1.Controllers
{

    /// <summary>
    /// test
    /// </summary>
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestsController : ControllerBase
    {

        /// <summary>
        /// get current data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<DateTime>(StatusCodes.Status200OK)]
        public IActionResult Get()
            => Ok(DateTime.UtcNow);

        /// <summary>
        /// check token
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CheckToken()
            => NoContent();
    }
}
