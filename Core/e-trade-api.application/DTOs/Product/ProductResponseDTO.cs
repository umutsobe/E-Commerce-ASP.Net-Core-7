namespace e_trade_api.application;

public class ProductResponseDTO
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public float Price { get; set; }
    public string Url { get; set; }
    public string? ProductImageShowCasePath { get; set; }

    //rating
    public double AverageStar { get; set; }
    public int TotalRatingNumber { get; set; }
}
