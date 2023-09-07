using e_trade_api.application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_trade_api.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Admin")]
public class BasketsController : ControllerBase
{
    readonly IMediator _mediator;
    readonly IBasketService _basketService;

    public BasketsController(IMediator mediator, IBasketService basketService)
    {
        _mediator = mediator;
        _basketService = basketService;
    }

    [HttpGet("{basketId}")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Baskets,
        ActionType = ActionType.Reading,
        Definition = "Get Basket Items"
    )]
    public async Task<IActionResult> GetBasketItems([FromRoute] string basketId)
    {
        List<BasketItemDTO> response = await _basketService.GetBasketItemsAsync(basketId);
        return Ok(response);
    }

    [HttpPost]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Baskets,
        ActionType = ActionType.Writing,
        Definition = "Add Item To Basket"
    )]
    public async Task<IActionResult> AddItemToBasket(
        AddItemToBasketCommandRequest addItemToBasketCommandRequest
    )
    {
        AddItemToBasketCommandResponse response = await _mediator.Send(
            addItemToBasketCommandRequest
        );
        return Ok(response);
    }

    [HttpPut]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Baskets,
        ActionType = ActionType.Updating,
        Definition = "Update Quantity"
    )]
    public async Task<IActionResult> UpdateQuantity(
        UpdateBasketItemQuantityCommandRequest updateQuantityCommandRequest
    )
    {
        UpdateBasketItemQuantityCommandResponse response = await _mediator.Send(
            updateQuantityCommandRequest
        );
        return Ok(response);
    }

    [HttpDelete("{BasketItemId}")]
    [AuthorizeDefinition(
        Menu = AuthorizeDefinitionConstants.Baskets,
        ActionType = ActionType.Deleting,
        Definition = "Remove Basket Item"
    )]
    public async Task<IActionResult> RemoveBasketItem(
        [FromRoute] RemoveBasketItemCommandRequest removeBasketItemCommandRequest
    )
    {
        RemoveBasketItemCommandResponse response = await _mediator.Send(
            removeBasketItemCommandRequest
        );
        return Ok(response);
    }
}
