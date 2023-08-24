using e_trade_api.domain;

namespace e_trade_api.application;

public class CreateOrder
{
    public string UserId { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public List<CreateOrderItem> OrderItems { get; set; }
}
