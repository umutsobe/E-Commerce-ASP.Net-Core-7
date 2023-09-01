using System.Runtime.CompilerServices;
using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Admin")]
public class OrderController : ControllerBase
{
    readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Orders,
        ActionType = ActionType.Writing,
        Definition = "Create Order"
    )]
    public async Task<IActionResult> CreateOrder(
        CreateOrderCommandRequest createOrderCommandRequest
    )
    {
        CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);

        return Ok(response);
    }

    [HttpGet]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Orders,
        ActionType = ActionType.Reading,
        Definition = "Get All Orders"
    )]
    public async Task<IActionResult> GetAllOrders(
        [FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest
    )
    {
        GetAllOrdersQueryResponse response = await _mediator.Send(getAllOrdersQueryRequest);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Orders,
        ActionType = ActionType.Reading,
        Definition = "Get Order By Id"
    )]
    public async Task<ActionResult> GetOrderById(
        [FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest
    )
    {
        GetOrderByIdQueryResponse response = await _mediator.Send(getOrderByIdQueryRequest);
        return Ok(response);
    }

    [HttpGet("complete-order/{Id}")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Orders,
        ActionType = ActionType.Updating,
        Definition = "Complete Order"
    )]
    public async Task<IActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest request)
    {
        CompleteOrderCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}
