namespace e_trade_api.application;

public class ListUser
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public bool TwoFactorEnabled { get; set; }
}
