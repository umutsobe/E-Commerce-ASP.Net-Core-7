namespace e_trade_api.application;

public class GetAllProductQueryResponse
{
    public int TotalProductCount { get; set; }
    public List<ProductResponseDTO> Products { get; set; }
}
