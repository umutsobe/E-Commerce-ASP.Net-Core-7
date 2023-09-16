namespace e_trade_api.application;

public class CreateOrderResponseDTO
{
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string? OrderId { get; set; }
    public string? OrderCode { get; set; }
}
