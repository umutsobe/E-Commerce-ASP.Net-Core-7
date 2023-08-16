namespace e_trade_api.application;

public interface IUserService
{
    Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
}
