using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.Persistence;

public class AuthService : IAuthService
{
    readonly UserManager<AppUser> _userManager;
    readonly IMailService _mailService;

    public AuthService(UserManager<AppUser> userManager, IMailService mailService)
    {
        _userManager = userManager;
        _mailService = mailService;
    }

    public async Task PasswordResetAsync(string email)
    {
        AppUser user = await _userManager.FindByEmailAsync(email);
        if (user != null)
        {
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            resetToken = resetToken.UrlEncode();

            await _mailService.SendPasswordResetMailAsync(email, user.Id, resetToken);
        }
    }

    public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            resetToken = resetToken.UrlDecode();

            return await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword",
                resetToken
            );
        }
        return false;
    }
}
