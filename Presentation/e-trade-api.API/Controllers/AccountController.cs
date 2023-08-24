using e_trade_api.application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Reading,
        Definition = "Get User Details"
    )]
    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUserDetails(string userId)
    {
        ListUserDetailsDTO listUser = await _accountService.GetUserDetails(userId);

        return Ok(listUser);
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Updating,
        Definition = "Update User Email"
    )]
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateUserEmail([FromBody] UseEmailUpdateDTO model)
    {
        bool response = await _accountService.UpdateEmail(model.UserId, model.Email);
        return Ok(response);
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Updating,
        Definition = "Update User Name"
    )]
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateUserName([FromBody] UserNameUpdateDTO model)
    {
        bool response = await _accountService.UpdateName(model.UserId, model.Name);
        return Ok(response);
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Updating,
        Definition = "Get User Orders"
    )]
    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> ListUserOrders(string userId)
    {
        var orders = await _accountService.ListUserOrders(userId);
        return Ok(orders);
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Updating,
        Definition = "Update User Password"
    )]
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UserPasswordUpdate model)
    {
        Token token = await _accountService.UpdateUserPassword(model);
        token.Expiration = DateTime.Now;
        return Ok(token);
    }
}
