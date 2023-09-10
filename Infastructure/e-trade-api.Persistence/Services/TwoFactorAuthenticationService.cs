using System.Text;
using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class TwoFactorAuthenticationService : ITwoFactorAuthenticationService
{
    readonly IMailService _mailService;
    readonly ITwoFactorAuthenticationReadRepository _twoFactorAuthenticationReadRepository;
    readonly ITwoFactorAuthenticationWriteRepository _twoFactorAuthenticationWriteRepository;
    readonly UserManager<AppUser> _userManager;

    public TwoFactorAuthenticationService(
        IMailService mailService,
        ITwoFactorAuthenticationReadRepository twoFactorAuthenticationReadRepository,
        ITwoFactorAuthenticationWriteRepository twoFactorAuthenticationWriteRepository,
        UserManager<AppUser> userManager
    )
    {
        _mailService = mailService;
        _twoFactorAuthenticationReadRepository = twoFactorAuthenticationReadRepository;
        _twoFactorAuthenticationWriteRepository = twoFactorAuthenticationWriteRepository;
        _userManager = userManager;
    }

    public async Task<CreateCodeAndSendEmailResponse> CreateCodeAndSendEmail(string userId)
    {
        AppUser? appUser = await _userManager.FindByIdAsync(userId);

        if (appUser == null)
            return new() { Message = "Kullanıcı bulunamadı", Succeeded = false };

        if (appUser.EmailConfirmed)
            return new() { Message = "Emailiniz zaten onaylandı. Giriş yapın", Succeeded = false };

        TwoFactorAuthentication? isValid = //son 5 dakikada bu user'a ait kod üretildi mi?
        await _twoFactorAuthenticationReadRepository.Table
            .Where(t => t.UserId == userId && t.ExpirationDate > DateTime.UtcNow) //exp saat 12.05, datenow 12.04 ise true
            .FirstOrDefaultAsync();

        if (isValid != null) //son 5 dakikada kod oluşturmuş
        {
            return new() { Message = "Son 3 dakikada zaten kod oluşturuldu", Succeeded = false };
        }

        if (!await CheckCodeAttempts(userId))
            return new()
            {
                Message = "Çok fazla denemede bulundunuz. 24 saat sonra tekrar deneyin.",
                Succeeded = false
            };

        if (isValid == null)
        {
            string code = CreateCode();

            await _mailService.SendEmailVerificationCode(appUser.Email, code.ToString());

            await _twoFactorAuthenticationWriteRepository.AddAsync(
                new()
                {
                    Code = code.ToString(),
                    Id = Guid.NewGuid(),
                    ExpirationDate = DateTime.UtcNow.AddMinutes(3),
                    UserId = userId,
                    IsUsed = false
                }
            );

            await _twoFactorAuthenticationWriteRepository.SaveAsync();

            return new() { Message = "Kod başarıyla oluşturuldu", Succeeded = true };
        }
        return new();
    }

    public async Task<IsCodeValidResponseMessage> IsCodeValid(IsCodeValidRequest model) //buraya sadece şifreyle kaydolmuş biri gelebilir. 24 saat içinde 5 deneme sonrası artık kod üretilmemeli
    {
        AppUser? appUser = await _userManager.FindByIdAsync(model.UserId);

        if (appUser == null)
            return new() { Message = "Kullanıcı bulunamadı", Succeeded = false };

        if (appUser.EmailConfirmed)
            return new() { Message = "Emailiniz zaten onaylandı. Giriş yapın", Succeeded = false };

        if (!await CheckCodeAttempts(model.UserId))
            return new()
            {
                Message = "Çok fazla denemede bulundunuz. 24 saat sonra tekrar deneyin.",
                Succeeded = false
            };

        TwoFactorAuthentication? twoFactor = await _twoFactorAuthenticationReadRepository.Table
            .Where(
                t =>
                    t.UserId == model.UserId
                    && t.Code == model.Code
                    && t.IsUsed == false
                    && t.ExpirationDate > DateTime.UtcNow //exp saat 12.05, datenow 12.04 ise true
            )
            .FirstOrDefaultAsync();

        if (twoFactor != null) //gelen kod valid
        {
            appUser.EmailConfirmed = true;

            twoFactor.IsUsed = true;
            await _twoFactorAuthenticationWriteRepository.SaveAsync();
            await _userManager.UpdateAsync(appUser);

            //kodları kaldırma
            List<TwoFactorAuthentication> twoFactorAuthentications =
                await _twoFactorAuthenticationReadRepository.Table
                    .Where(t => t.UserId == model.UserId)
                    .ToListAsync();
            _twoFactorAuthenticationWriteRepository.RemoveRange(twoFactorAuthentications);
            await _twoFactorAuthenticationWriteRepository.SaveAsync();

            return new()
            {
                Message = "Mailiniz başarıyla onaylandı. Lütfen giriş yapın",
                Succeeded = true
            };
        }

        return new() { Message = "Girdiğiniz kod hatalı. Tekrar deneyiniz", Succeeded = false };
    }

    private async Task<bool> CheckCodeAttempts(string userId)
    {
        // Kullanıcıya ait kod deneme sayacını ve son deneme tarihini veritabanından alın
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false; // Kullanıcı bulunamadı, işlem başarısız
        }

        int maxAttempts = 10; // Maksimum deneme sayısı
        int timeWindowInMinutes = 24 * 60; // Zaman penceresi (örneğin, 24 saat)

        DateTime currentTime = DateTime.UtcNow;
        DateTime lastAttemptTime = user.LastCodeAttemptTime.HasValue
            ? user.LastCodeAttemptTime.Value
            : DateTime.MinValue;

        int codeAttemptCount = user.CodeAttemptCount;

        // Kullanıcının son deneme tarihi belirli bir zaman penceresinden (örneğin, 24 saat) önceyse sıfırla
        if ((currentTime - lastAttemptTime).TotalMinutes > timeWindowInMinutes)
        {
            user.LastCodeAttemptTime = currentTime;
            user.CodeAttemptCount = 1;
        }
        else
        {
            user.CodeAttemptCount++;

            if (codeAttemptCount >= maxAttempts)
            {
                return false; // deneme yapamaz
            }
        }

        // Kullanıcı bilgilerini güncelle
        await _userManager.UpdateAsync(user);

        return true; // Kullanıcı hala deneme yapabilir
    }

    public async Task<bool> IsUserEmailConfirmed(string userId)
    {
        AppUser? appUser = await _userManager.FindByIdAsync(userId);

        if (appUser == null)
            return false;

        if (appUser.EmailConfirmed)
            return true;

        return false;
    }

    private string CreateCode()
    {
        int length = 6;
        Random random = new();
        const string characters = "ABCDEFGHJKLMNPRSTUWYZ123456789";
        StringBuilder code = new(length);

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(characters.Length);
            code.Append(characters[index]);
        }
        return code.ToString();
    }
}
