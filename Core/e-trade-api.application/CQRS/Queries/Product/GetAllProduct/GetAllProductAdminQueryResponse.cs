namespace e_trade_api.application;

public class GetAllProductAdminQueryResponse
{
    public int TotalProductCount { get; set; }
    public List<ProductResponseAdminDTO> Products { get; set; }
}
