using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        readonly IConfiguration _configuration;

        public FilesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public IActionResult GetBaseStorageUrl()
        {
            return Ok(new { Url = _configuration["BaseStorageUrl"] });
        }
    }
}
