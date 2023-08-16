using System.Runtime.CompilerServices;
using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(AuthenticationSchemes = "Admin")]
public class OrderController : ControllerBase
{
    readonly IMediator _mediator;
    readonly IOrderService _orderService;

    public OrderController(IMediator mediator, IOrderService orderService)
    {
        _mediator = mediator;
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        CreateOrderCommandRequest createOrderCommandRequest
    )
    {
        CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders(
        [FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest
    )
    {
        GetAllOrdersQueryResponse response = await _mediator.Send(getAllOrdersQueryRequest);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult> GetOrderById(
        [FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest
    )
    {
        GetOrderByIdQueryResponse response = await _mediator.Send(getOrderByIdQueryRequest);
        return Ok(response);
    }

    [HttpGet("complete-order/{Id}")]
    public async Task<IActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest request)
    {
        CompleteOrderCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}
