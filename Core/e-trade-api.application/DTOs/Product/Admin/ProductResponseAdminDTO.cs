namespace e_trade_api.application;

public class ProductResponseAdminDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public string Url { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public List<ProductImageFileResponseDTO> ProductImageFiles { get; set; }

    //admin
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public int TotalBasketAdded { get; set; }
    public int TotalOrderNumber { get; set; }

    //rating
    public double AverageStar { get; set; }
    public int TotalRatingNumber { get; set; }
}
