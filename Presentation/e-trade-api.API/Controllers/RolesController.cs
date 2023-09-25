using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    // [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles", Menu = "Roles")]
    public async Task<IActionResult> GetRoles([FromQuery] GetRolesQueryRequest getRolesQueryRequest)
    {
        GetRolesQueryResponse response = await _mediator.Send(getRolesQueryRequest);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    // [AuthorizeDefinition(
    //     ActionType = ActionType.Reading,
    //     Definition = "Get Role By Id",
    //     Menu = "Roles"
    // )]
    public async Task<IActionResult> GetRoles(
        [FromRoute] GetRoleByIdQueryRequest getRoleByIdQueryRequest
    )
    {
        GetRoleByIdQueryResponse response = await _mediator.Send(getRoleByIdQueryRequest);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        ActionType = ActionType.Writing,
        Definition = "Create Role",
        Menu = "Roles"
    )]
    public async Task<IActionResult> CreateRole(
        [FromBody] CreateRoleCommandRequest createRoleCommandRequest
    )
    {
        CreateRoleCommandResponse response = await _mediator.Send(createRoleCommandRequest);
        return Ok(response);
    }

    [HttpPut("{Id}")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        ActionType = ActionType.Updating,
        Definition = "Update Role",
        Menu = "Roles"
    )]
    public async Task<IActionResult> UpdateRole(
        [FromBody, FromRoute] UpdateRoleCommandRequest updateRoleCommandRequest
    )
    {
        UpdateRoleCommandResponse response = await _mediator.Send(updateRoleCommandRequest);
        return Ok(response);
    }

    [HttpDelete("{Id}")]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        ActionType = ActionType.Deleting,
        Definition = "Delete Role",
        Menu = "Roles"
    )]
    public async Task<IActionResult> DeleteRole(
        [FromRoute] DeleteRoleCommandRequest deleteRoleCommandRequest
    )
    {
        DeleteRoleCommandResponse response = await _mediator.Send(deleteRoleCommandRequest);
        return Ok(response);
    }
}
