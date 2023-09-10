namespace e_trade_api.application;

public class CreateUserResponseDTO
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public string? UserId { get; set; }
}
