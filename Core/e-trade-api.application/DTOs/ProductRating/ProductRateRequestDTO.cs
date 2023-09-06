namespace e_trade_api.application;

public class ProductRateRequestDTO
{
    public string ProductId { get; set; }
    public string UserId { get; set; }
    public int Star { get; set; }
    public string Comment { get; set; }
}
