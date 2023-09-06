namespace e_trade_api.application;

public class GetProductRatingsRequestDTO
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 12;
    public string ProductId { get; set; }
    public string? SortType { get; set; }
    public string? Keyword { get; set; }
}
