namespace e_trade_api.application;

public class GetAllProductsResponseDTO
{
    public int TotalProductCount { get; set; }
    public List<ProductResponseDTO> Products { get; set; }
}
