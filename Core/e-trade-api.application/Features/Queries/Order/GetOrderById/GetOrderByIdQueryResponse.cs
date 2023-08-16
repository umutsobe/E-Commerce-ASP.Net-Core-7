namespace e_trade_api.application;

public class GetOrderByIdQueryResponse
{
    public string Address { get; set; }
    public object OrderItems { get; set; }
    public DateTime CreatedDate { get; set; }
    public string Description { get; set; }
    public string Id { get; set; }
    public string OrderCode { get; set; }
}
