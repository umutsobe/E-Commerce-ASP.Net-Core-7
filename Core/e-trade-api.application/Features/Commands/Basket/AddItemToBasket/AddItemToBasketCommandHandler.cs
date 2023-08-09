using MediatR;

namespace e_trade_api.application;

public class AddItemToBasketCommandHandler
    : IRequestHandler<AddItemToBasketCommandRequest, AddItemToBasketCommandResponse>
{
    readonly IBasketService _basketService;

    public AddItemToBasketCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<AddItemToBasketCommandResponse> Handle(
        AddItemToBasketCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _basketService.AddItemToBasketAsync(
            new() { ProductId = request.ProductId, Quantity = request.Quantity },
            request.BasketId
        );

        return new();
    }
}
