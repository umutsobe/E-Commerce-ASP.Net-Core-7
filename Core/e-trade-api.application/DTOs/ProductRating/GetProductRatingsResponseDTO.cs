namespace e_trade_api.application;

public class GetProductRatingsResponseDTO
{
    public int TotalProductRatingCount { get; set; }
    public List<GetProductRatingResponseDTO>? Ratings { get; set; }
}
