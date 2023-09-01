namespace e_trade_api.application;

public class GetProductsByFilterDTO
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 12;

    public string? Keyword { get; set; }
    public string? CategoryName { get; set; }
    public int? MinPrice { get; set; } = 0;
    public int? MaxPrice { get; set; } = int.MaxValue;
    public string? Sort { get; set; }
}
