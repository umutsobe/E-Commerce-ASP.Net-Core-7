using MediatR;

namespace e_trade_api.application;

public class RemoveBasketItemCommandRequest : IRequest<RemoveBasketItemCommandResponse>
{
    public string BasketItemId { get; set; }
}
