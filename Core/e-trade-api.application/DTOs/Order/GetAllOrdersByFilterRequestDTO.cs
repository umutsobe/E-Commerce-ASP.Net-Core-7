namespace e_trade_api.application;

public class GetAllOrdersByFilterRequestDTO
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 8;
    public string? OrderCodeKeyword { get; set; }
    public string? UsernameKeyword { get; set; }
    public bool? IsConfirmed { get; set; }
    public string? Sort { get; set; }
}
