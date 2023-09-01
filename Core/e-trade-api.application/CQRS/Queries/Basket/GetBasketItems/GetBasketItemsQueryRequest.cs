using MediatR;

namespace e_trade_api.application;

public class GetBasketItemsQueryRequest : IRequest<List<GetBasketItemsQueryResponse>>
{
    public string BasketId { get; set; }
}
