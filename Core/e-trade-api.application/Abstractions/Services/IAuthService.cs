namespace e_trade_api.application;

public interface IAuthService
{
    Task PasswordResetAsync(string email);
    Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
}
