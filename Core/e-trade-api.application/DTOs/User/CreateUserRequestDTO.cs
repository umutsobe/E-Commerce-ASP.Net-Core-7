namespace e_trade_api.application;

public class CreateUserRequestDTO
{
    public string Name { get; set; }

    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
}
