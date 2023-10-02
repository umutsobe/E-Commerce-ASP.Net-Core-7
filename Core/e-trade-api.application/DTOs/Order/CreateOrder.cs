namespace e_trade_api.application;

public class CreateOrder
{
    public string Description { get; set; }
    public string Address { get; set; }
    public List<CreateOrderItem> OrderItems { get; set; }
}
