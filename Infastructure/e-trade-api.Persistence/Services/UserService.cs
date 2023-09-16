using System.Security;
using e_trade_api.application;
using e_trade_api.domain;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence;

public class UserService : IUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly IEndpointReadRepository _endpointReadRepository;
    readonly IBasketService _basketService;
    readonly ITokenHandler _tokenHandler;
    readonly SignInManager<AppUser> _signInManager;
    readonly ITwoFactorAuthenticationService _twoFactorAuthenticationService;

    public UserService(
        UserManager<AppUser> userManager,
        IEndpointReadRepository endpointReadRepository,
        IBasketService basketService,
        ITokenHandler tokenHandler,
        SignInManager<AppUser> signInManager,
        ITwoFactorAuthenticationService twoFactorAuthenticationService
    )
    {
        _userManager = userManager;
        _endpointReadRepository = endpointReadRepository;
        _basketService = basketService;
        _tokenHandler = tokenHandler;
        _signInManager = signInManager;
        _twoFactorAuthenticationService = twoFactorAuthenticationService;
    }

    public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
    {
        AppUser? user = await _userManager.FindByIdAsync(userId);

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

    public async Task<List<ListUserDTO>> GetAllUsersAsync(int page, int size)
    {
        var users = await _userManager.Users.Skip(page * size).Take(size).ToListAsync();

        return users
            .Select(
                user =>
                    new ListUserDTO
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
        AppUser? user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            await _userManager.AddToRolesAsync(user, roles);
        }
    }

    public async Task<string[]> GetRolesToUserAsync(string userIdOrName)
    {
        AppUser? user = await _userManager.FindByIdAsync(userIdOrName);
        if (user == null)
            user = await _userManager.FindByNameAsync(userIdOrName);

        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            return userRoles.ToArray();
        }
        return new string[] { };
    }

    public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
    {
        var userRoles = await GetRolesToUserAsync(name);

        if (!userRoles.Any())
            return false;

        Endpoint? endpoint = await _endpointReadRepository.Table
            .Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Code == code);

        if (endpoint == null)
            return false;

        var endpointRoles = endpoint.Roles.Select(r => r.Name);

        foreach (var userRole in userRoles)
        {
            foreach (var endpointRole in endpointRoles)
                if (userRole == endpointRole)
                    return true;
        }

        return false;
    }

    //old
    public async Task<CreateUserResponseDTO> CreateUser(CreateUserRequestDTO model)
    {
        AppUser? appuser = await _userManager.FindByEmailAsync(model.Email);
        if (appuser != null)
        {
            return new()
            {
                Succeeded = false,
                Message =
                    "Bu e-posta adresiyle zaten bir hesap oluşturulmuş. Lütfen farklı bir e-posta adresiyle kaydolmayı deneyin veya mevcut hesabınızla giriş yapın."
            };
        }

        appuser = await _userManager.FindByNameAsync(model.UserName);

        if (appuser != null)
        {
            return new()
            {
                Succeeded = false,
                Message =
                    "Bu kullanıcı adıyla zaten bir hesap oluşturulmuş. Lütfen farklı bir kullanıcı adıyla kaydolmayı deneyin veya mevcut hesabınızla giriş yapın."
            };
        }

        AppUser user =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
            };

        IdentityResult userResult = await _userManager.CreateAsync(user, model.Password);

        CreateUserCommandResponse response = new();

        if (userResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "normalUser");
            await _basketService.CreateBasket(user.Id.ToString());

            //two factor email gönderme

            //code veritabanına kaydetme

            response.Succeeded = true;
            response.Message = "Kullanıcı Başarıyla Oluşturulmuştur. Email'inizi doğrulayın";
            response.UserId = user.Id;
        }
        else
        {
            response.Succeeded = false;
            response.Message = "Kullanıcı oluşturulurken bir hatayla karşılaşıldı.";
            response.UserId = null;
        }

        return new()
        {
            Message = response.Message,
            Succeeded = response.Succeeded,
            UserId = response.UserId
        };
    }

    public async Task<Token> GoogleLogin(GoogleLoginRequestDTO model)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { MyConfigurationManager.GetGoogleCredential() }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings);

            var user = await _userManager.FindByLoginAsync(model.Provider, payload.Subject);
            var userId = user != null ? user.Id : null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(payload.Email);

                if (user == null)
                {
                    user = new AppUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = payload.Email,
                        UserName = payload.Email,
                        Name = payload.Name,
                        EmailConfirmed = true
                    };

                    userId = user.Id;
                    var identityResult = await _userManager.CreateAsync(user);

                    await _basketService.CreateBasket(user.Id.ToString());

                    if (!identityResult.Succeeded)
                    {
                        throw new Exception("Could not create a new user.");
                    }
                }
            }

            await _userManager.AddLoginAsync(
                user,
                new UserLoginInfo(model.Provider, payload.Subject, model.Provider)
            );

            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Count == 0)
                await _userManager.AddToRoleAsync(user, "normalUser");

            Token token = await _tokenHandler.CreateAccessToken(180, userId.ToString());

            return token;
        }
        catch (Exception ex)
        {
            // Log the error and handle it appropriately.
            // Return a meaningful error response to the client.
            throw new Exception("External authentication error.", ex);
        }
    }

    public async Task<LoginUserCommandResponse> LoginUser(LoginUserRequestDTO model)
    {
        AppUser? user = await _userManager.FindByNameAsync(model.EmailOrUserName); // username veya email olan string ifadeyi ilk username üzerinden kontrol ettik
        if (user == null)
        { // eğer böyle bir username yoksa email olarak kontrol ettik
            user = await _userManager.FindByEmailAsync(model.EmailOrUserName);
        }

        if (user == null) // böyle bir email de yoksa böyle bir kullanıcı yoktur. hata fırlattık
            return new LoginUserErrorCommandResponse() { Message = "Hesap bulunamadı" };

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(
            user,
            model.Password,
            false
        );

        if (signInResult.Succeeded)
        {
            if (await _twoFactorAuthenticationService.IsUserEmailConfirmed(user.Id))
            {
                //yetki belirleme
                Token token = await _tokenHandler.CreateAccessToken(180, user.Id);

                return new LoginUserSuccessCommandResponse() { Token = token };
            }
            else
            {
                return new LoginUserSuccessButEmailNotConfirmed()
                {
                    UserId = user.Id,
                    AuthMessage =
                        "Girdiğiniz bilgiler doğru. Siteye girmek için lütfen Mail'inizi doğrulayın"
                };
            }
        }

        return new LoginUserErrorCommandResponse() { Message = "Şifre Hatalı" };
    }
}
