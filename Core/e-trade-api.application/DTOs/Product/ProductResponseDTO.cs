namespace e_trade_api.application;

public class ProductResponseDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public string Url { get; set; }
    public object ProductImageFiles { get; set; }
}
// p.Id,
// p.Name,
// p.Stock,
// p.Price,
// p.CreatedDate,
// p.UpdatedDate,
// p.ProductImageFiles
