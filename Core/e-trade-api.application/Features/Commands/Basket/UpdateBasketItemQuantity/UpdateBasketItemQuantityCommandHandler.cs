using MediatR;

namespace e_trade_api.application;

public class UpdateQuantityCommandHandler
    : IRequestHandler<
        UpdateBasketItemQuantityCommandRequest,
        UpdateBasketItemQuantityCommandResponse
    >
{
    readonly IBasketService _basketService;

    public UpdateQuantityCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<UpdateBasketItemQuantityCommandResponse> Handle(
        UpdateBasketItemQuantityCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _basketService.UpdateQuantityAsync(
            new() { BasketItemId = request.BasketItemId, Quantity = request.Quantity }
        );

        return new();
    }
}
