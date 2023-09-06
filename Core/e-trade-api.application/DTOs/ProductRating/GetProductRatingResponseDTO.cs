namespace e_trade_api.application;

public class GetProductRatingResponseDTO
{
    public string UserName { get; set; }
    public string Comment { get; set; }
    public int Star { get; set; }
    public DateTime CreatedDate { get; set; }
}
