namespace e_trade_api.application;

public class CreateUserCommandResponse
{
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public string? UserId { get; set; }
}
