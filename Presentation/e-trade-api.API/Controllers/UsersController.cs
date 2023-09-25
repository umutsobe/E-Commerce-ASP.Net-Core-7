using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[Route("api/[controller]")]
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

    [HttpPost("password-reset")]
    public async Task<IActionResult> PasswordReset( //email girip emaile kod gitmesi (unuttum kısmı)
        [FromBody] PasswordResetCommandRequest passwordResetCommandRequest
    )
    {
        PasswordResetCommandResponse response = await _mediator.Send(passwordResetCommandRequest);
        return Ok(response);
    }

    [HttpPost("verify-reset-token")]
    public async Task<IActionResult> VerifyResetToken( //passwork update sayfasına giriş kontrolü
        [FromBody] VerifyResetTokenCommandRequest verifyResetTokenCommandRequest
    )
    {
        VerifyResetTokenCommandResponse response = await _mediator.Send(
            verifyResetTokenCommandRequest
        );
        return Ok(response);
    }

    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword( //şifre değiştirme (unuttum kısmı)
        [FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest
    )
    {
        UpdatePasswordCommandResponse response = await _mediator.Send(updatePasswordCommandRequest);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        ActionType = ActionType.Reading,
        Definition = "Get All Users",
        Menu = "Users"
    )]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] GetAllUsersQueryRequest getAllUsersQueryRequest
    )
    {
        GetAllUsersQueryResponse response = await _mediator.Send(getAllUsersQueryRequest);
        return Ok(response);
    }

    [HttpGet("get-roles-to-user/{UserId}")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        ActionType = ActionType.Reading,
        Definition = "Get Roles To Users",
        Menu = "Users"
    )]
    public async Task<IActionResult> GetRolesToUser(
        [FromRoute] GetRolesToUserQueryRequest getRolesToUserQueryRequest
    )
    {
        GetRolesToUserQueryResponse response = await _mediator.Send(getRolesToUserQueryRequest);
        return Ok(response);
    }

    [HttpPost("assign-role-to-user")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        ActionType = ActionType.Reading,
        Definition = "Assign Role To User",
        Menu = "Users"
    )]
    public async Task<IActionResult> AssignRoleToUser(
        AssignRoleToUserCommandRequest assignRoleToUserCommandRequest
    )
    {
        AssignRoleToUserCommandResponse response = await _mediator.Send(
            assignRoleToUserCommandRequest
        );
        return Ok(response);
    }
}
