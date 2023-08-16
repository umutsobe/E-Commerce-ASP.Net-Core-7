using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[Route("api/[controller]")]
// [Authorize]
[ApiController]
public class UsersController : ControllerBase
{
    readonly IMediator _mediator;
    readonly IMailService _mailService;

    public UsersController(IMediator mediator, IMailService mailService)
    {
        _mediator = mediator;
        _mailService = mailService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommandRequest createUserCommandRequest)
    {
        CreateUserCommandResponse response = await _mediator.Send(createUserCommandRequest);
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
    {
        LoginUserCommandResponse response = await _mediator.Send(loginUserCommandRequest);
        return Ok(response);
    }

    [HttpPost("googlelogin")]
    public async Task<IActionResult> GoogleLogin(
        GoogleLoginCommandRequest googleLoginCommandRequest
    )
    {
        GoogleLoginCommandResponse response = await _mediator.Send(googleLoginCommandRequest);
        return Ok(response);
    }

    // [HttpGet]
    // public async Task<IActionResult> ExampleMailTest()
    // {
    //     await _mailService.SendMailAsync(
    //         "umuttsobeksk@gmail.com",
    //         "Örnek Mail",
    //         "<strong>Bu bir örnek maildir.</strong>"
    //     );
    //     return Ok();
    // }

    [HttpPost("password-reset")]
    public async Task<IActionResult> PasswordReset(
        [FromBody] PasswordResetCommandRequest passwordResetCommandRequest
    )
    {
        PasswordResetCommandResponse response = await _mediator.Send(passwordResetCommandRequest);
        return Ok(response);
    }

    [HttpPost("verify-reset-token")]
    public async Task<IActionResult> VerifyResetToken(
        [FromBody] VerifyResetTokenCommandRequest verifyResetTokenCommandRequest
    )
    {
        VerifyResetTokenCommandResponse response = await _mediator.Send(
            verifyResetTokenCommandRequest
        );
        return Ok(response);
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword(
        [FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest
    )
    {
        UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
        return Ok(response);
    }
}
