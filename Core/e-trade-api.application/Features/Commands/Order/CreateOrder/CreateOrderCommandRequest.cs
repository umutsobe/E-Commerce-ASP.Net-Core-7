using MediatR;

namespace e_trade_api.application;

public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
{
    public string UserId { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public List<CreateOrderItem> OrderItems { get; set; }
}
