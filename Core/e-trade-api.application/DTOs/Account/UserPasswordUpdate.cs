namespace e_trade_api.application;

public class UserPasswordUpdate
{
    public string UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
