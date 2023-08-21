using e_trade_api.application;
using e_trade_api.Infastructure;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("[action]")]
    public async Task<IActionResult> GetClientUrl()
    {
        string ClientUrl = MyConfigurationManager.GetClientUrl();
        return Ok(ClientUrl);
    }
}
