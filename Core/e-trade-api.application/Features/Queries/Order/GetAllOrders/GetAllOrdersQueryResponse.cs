namespace e_trade_api.application;

public class GetAllOrdersQueryResponse
{
    public int TotalOrderCount { get; set; }
    public object Orders { get; set; }
}
