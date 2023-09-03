namespace e_trade_api.application;

public class GetProductByIdDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public string Url { get; set; }
    public string Description { get; set; }
    public object ProductImageFiles { get; set; }
}
