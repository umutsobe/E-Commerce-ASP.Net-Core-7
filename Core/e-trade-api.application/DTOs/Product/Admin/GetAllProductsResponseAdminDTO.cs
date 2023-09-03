namespace e_trade_api.application;

public class GetAllProductsResponseAdminDTO
{
    public int TotalProductCount { get; set; }
    public List<ProductResponseAdminDTO> Products { get; set; }
}
