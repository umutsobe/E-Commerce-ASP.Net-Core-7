using e_trade_api.application;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult GetBaseStorageUrl()
        {
            return Ok(new { Url = MyConfigurationManager.GetBaseAzureStorageUrl() });
        }
    }
}
