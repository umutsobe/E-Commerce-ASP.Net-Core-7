using MediatR;

namespace e_trade_api.application;

public class RemoveBasketItemCommandHandler
    : IRequestHandler<RemoveBasketItemCommandRequest, RemoveBasketItemCommandResponse>
{
    readonly IBasketService _basketService;

    public RemoveBasketItemCommandHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<RemoveBasketItemCommandResponse> Handle(
        RemoveBasketItemCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _basketService.RemoveBasketItemAsync(request.BasketItemId);
        return new();
    }
}
