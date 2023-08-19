using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
    {
        var users = await _userManager.Users.Skip(page * size).Take(size).ToListAsync();

        return users
            .Select(
                user =>
                    new ListUser
                    {
                        Id = user.Id,
                        Email = user.Email,
                        TwoFactorEnabled = user.TwoFactorEnabled,
                        UserName = user.UserName
                    }
            )
            .ToList();
    }

    public int TotalUsersCount => _userManager.Users.Count();

    public async Task AssignRoleToUserAsnyc(string userId, string[] roles)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRolesAsync(user, roles);
        }
    }

    public async Task<string[]> GetRolesToUserAsync(string userId)
    {
        AppUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }
        return new string[] { };
    }
}
