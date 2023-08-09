using MediatR;

namespace e_trade_api.application;

public class AddItemToBasketCommandRequest : IRequest<AddItemToBasketCommandResponse>
{
    public string BasketId { get; set; }
    public string ProductId { get; set; }
    public int Quantity { get; set; }
}
