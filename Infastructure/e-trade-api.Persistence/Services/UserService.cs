using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.Persistence;

public class UserService : IUserService
{
    readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);

        if (user != null)
        {
            resetToken = resetToken.UrlDecode();
            IdentityResult result = await _userManager.ResetPasswordAsync(
                user,
                resetToken,
                newPassword
            );
            if (result.Succeeded)
                await _userManager.UpdateSecurityStampAsync(user); // şifre değişti şimdi securitystamp şifresini ezmemiz gerekiyor. burada update ediyoruz. reset token bir daha kullanılmayacak
            else
                throw new Exception("Şifre Değiştirilirken bir hata meydana geldi");
        }
    }
}
