namespace e_trade_api.application;

public class GetUserRatingsRequestDTO
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 12;
    public string UserId { get; set; }
    public string? SortType { get; set; }
}
