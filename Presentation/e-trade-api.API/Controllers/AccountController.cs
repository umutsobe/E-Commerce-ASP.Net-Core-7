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
        token.Expiration = DateTime.UtcNow;
        return Ok(token);
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Writing,
        Definition = "Add User Address"
    )]
    [HttpPost("[action]")]
    public async Task<IActionResult> AddUserAddress([FromBody] CreateUserAddress model) //ok
    {
        await _accountService.AddUserAddress(model);

        return Ok();
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Reading,
        Definition = "Get User Addresess"
    )]
    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetUserAddresses([FromRoute] string userId)
    {
        List<GetUserAddress> response = await _accountService.GetUserAddresses(userId);

        return Ok(response);
    }

    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Deleting,
        Definition = "Delete User Address"
    )]
    [HttpDelete("[action]/{addressId}")]
    public async Task<IActionResult> DeleteUserAdsress([FromRoute] string addressId)
    {
        await _accountService.DeleteUserAdsress(addressId);

        return Ok();
    }

    // [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Updating,
        Definition = "Update User Email Step 1"
    )]
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateEmailStep1([FromBody] UpdateUserEmailRequestDTO model)
    {
        CreateCodeAndSendEmailResponse response = await _accountService.UpdateEmailStep1(model);
        return Ok(response);
    }

    // [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(
        Menu = "Account",
        ActionType = ActionType.Updating,
        Definition = "Update User Email Step 2"
    )]
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateEmailStep2([FromBody] UpdateEmailStep2 model)
    {
        CreateCodeAndSendEmailResponse response = await _accountService.UpdateEmailStep2(model);
        return Ok(response);
    }
}
