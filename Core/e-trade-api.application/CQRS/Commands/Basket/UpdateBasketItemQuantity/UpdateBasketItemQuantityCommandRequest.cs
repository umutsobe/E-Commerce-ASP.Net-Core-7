using MediatR;

namespace e_trade_api.application;

public class UpdateBasketItemQuantityCommandRequest
    : IRequest<UpdateBasketItemQuantityCommandResponse>
{
    public string BasketItemId { get; set; }
    public int Quantity { get; set; }
}
