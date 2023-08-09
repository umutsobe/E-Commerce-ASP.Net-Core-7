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
    public async Task<IActionResult> CreateOrder(
        CreateOrderCommandRequest createOrderCommandRequest
    )
    {
        CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);

        return Ok(response);
    }
}
