namespace e_trade_api.application;

public class GetUserRatingsResponseDTO
{
    public int TotaluserRatingCount { get; set; }
    public List<GetUserRatingResponseDTO> Ratings { get; set; }
}
