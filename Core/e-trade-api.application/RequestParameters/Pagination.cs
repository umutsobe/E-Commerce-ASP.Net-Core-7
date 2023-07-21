namespace e_trade_api.application;

public record Pagination
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
