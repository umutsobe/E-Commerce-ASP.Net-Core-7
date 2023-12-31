using System.Runtime.CompilerServices;
using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize(AuthenticationSchemes = "Auth")]
public class OrderController : ControllerBase
{
    readonly IMediator _mediator;
    readonly IOrderService _orderService;

    public OrderController(IMediator mediator, IOrderService orderService)
    {
        _mediator = mediator;
        _orderService = orderService;
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Auth")]
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

    [HttpGet("[action]")]
    // [AuthorizeDefinition(
    //     Menu = AuthorizeDefinitionConstants.Orders,
    //     ActionType = ActionType.Reading,
    //     Definition = "Get All Orders"
    // )]
    public async Task<IActionResult> GetAllOrdersByFilter(
        [FromQuery] GetAllOrdersByFilterRequestDTO model
    )
    {
        GetAllOrdersByFilterResponseDTO response = await _orderService.GetAllOrdersByFilterAsync(
            model
        );
        return Ok(response);
    }

    [HttpGet("{Id}")]
    // [AuthorizeDefinition(
    //     Menu = AuthorizeDefinitionConstants.Orders,
    //     ActionType = ActionType.Reading,
    //     Definition = "Get Order By Id"
    // )]
    public async Task<ActionResult> GetOrderById(
        [FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest
    )
    {
        GetOrderByIdQueryResponse response = await _mediator.Send(getOrderByIdQueryRequest);
        return Ok(response);
    }

    [HttpGet("complete-order/{Id}")]
    [Authorize(AuthenticationSchemes = "Auth")]
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

    [HttpGet("[action]/{orderCode}")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Orders,
        ActionType = ActionType.Reading,
        Definition = "Is Order Valid for Success Page"
    )]
    public async Task<IActionResult> IsOrderValid([FromRoute] string orderCode)
    {
        bool isValid = await _orderService.IsOrderValid(orderCode);

        var response = new { isValid = isValid };

        return Ok(response);
    }
}
