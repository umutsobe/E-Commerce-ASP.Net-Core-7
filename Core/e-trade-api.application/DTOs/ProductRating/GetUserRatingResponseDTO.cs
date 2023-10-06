namespace e_trade_api.application;

public class GetUserRatingResponseDTO
{
    public int Star { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedDate { get; set; }
    public string ProductUrlId { get; set; }
    public string ProductName { get; set; }
    public string ProductShowcaseImageUrl { get; set; }
}
