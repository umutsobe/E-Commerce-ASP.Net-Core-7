namespace e_trade_api.application;

public class AddProductsToCategoryRequestDTO
{
    public string CategoryId { get; set; }
    public List<ProductIdDTO> Products { get; set; }
}
// Guid categoryId, List<ProductDTO> products
