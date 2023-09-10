namespace e_trade_api.application;

public class UpdateUserEmailRequestDTO
{
    public string UserId { get; set; }
    public string Password { get; set; }
    public string NewEmail { get; set; }
}
