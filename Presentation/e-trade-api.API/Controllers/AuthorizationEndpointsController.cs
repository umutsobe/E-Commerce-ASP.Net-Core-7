using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationEndpointsController : ControllerBase
{
    readonly IMediator _mediator;

    public AuthorizationEndpointsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> GetRolesToEndpoint(
        GetRolesToEndpointQueryRequest rolesToEndpointQueryRequest
    )
    {
        GetRolesToEndpointQueryResponse response = await _mediator.Send(
            rolesToEndpointQueryRequest
        );
        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Auth")]
    [AuthorizeDefinition(
        Menu = "AuthorizationEndpoints",
        ActionType = ActionType.Writing,
        Definition = "Assign Role Endpoint"
    )]
    public async Task<IActionResult> AssignRoleEndpoint(
        AssignRoleEndpointCommandRequest assignRoleEndpointCommandRequest
    )
    {
        assignRoleEndpointCommandRequest.Type = typeof(Program);

        AssignRoleEndpointCommandResponse response = await _mediator.Send(
            assignRoleEndpointCommandRequest
        );
        return Ok(response);
    }
}
