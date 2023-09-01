using MediatR;

namespace e_trade_api.application;

public class GetBasketItemsQueryHandler
    : IRequestHandler<GetBasketItemsQueryRequest, List<GetBasketItemsQueryResponse>>
{
    readonly IBasketService _basketService;

    public GetBasketItemsQueryHandler(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<List<GetBasketItemsQueryResponse>> Handle(
        GetBasketItemsQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        var basketItems = await _basketService.GetBasketItemsAsync(request.BasketId);
        return basketItems
            .Select(
                ba =>
                    new GetBasketItemsQueryResponse
                    {
                        BasketItemId = ba.Id.ToString(),
                        ProductId = ba.ProductId.ToString(),
                        Name = ba.Product.Name,
                        Price = ba.Product.Price,
                        Quantity = ba.Quantity
                    }
            )
            .ToList();
    }
}
