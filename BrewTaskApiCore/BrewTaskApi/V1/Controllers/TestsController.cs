using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace BrewTaskApi.V1.Controllers
{

    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TestsController : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
            => Ok(DateTime.UtcNow);
    }
}
