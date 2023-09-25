using e_trade_api.application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TwoFactorAuthenticationController : ControllerBase
    {
        readonly ITwoFactorAuthenticationService _twoFactorAuthenticationService;

        public TwoFactorAuthenticationController(
            ITwoFactorAuthenticationService twoFactorAuthenticationService
        )
        {
            _twoFactorAuthenticationService = twoFactorAuthenticationService;
        }

        [HttpGet("[action]/{userId}")]
        [Authorize(AuthenticationSchemes = "Auth")]
        [AuthorizeDefinition(
            Menu = "TwoFactorAuthentication",
            ActionType = ActionType.Writing,
            Definition = "Create Code And Send Email"
        )]
        public async Task<IActionResult> CreateCodeAndSendEmail(string userId)
        {
            CreateCodeAndSendEmailResponse response =
                await _twoFactorAuthenticationService.CreateCodeAndSendEmail(userId);

            return Ok(response);
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Auth")]
        [AuthorizeDefinition(
            Menu = "TwoFactorAuthentication",
            ActionType = ActionType.Reading,
            Definition = "Is Code Valid"
        )]
        public async Task<IActionResult> IsCodeValid(IsCodeValidRequest model)
        {
            IsCodeValidResponseMessage response = await _twoFactorAuthenticationService.IsCodeValid(
                model
            );

            return Ok(response);
        }
    }
}
