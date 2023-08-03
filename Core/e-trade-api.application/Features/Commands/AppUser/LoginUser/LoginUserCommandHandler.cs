using e_trade_api.domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.application;

public class LoginUserCommandHandler
    : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    readonly UserManager<AppUser> _userManager;
    readonly SignInManager<AppUser> _signInManager;
    readonly ITokenHandler _tokenHandler;

    public LoginUserCommandHandler(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenHandler tokenHandler
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<LoginUserCommandResponse> Handle(
        LoginUserCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        AppUser user = await _userManager.FindByNameAsync(request.EmailOrUserName); // username veya email olan string ifadeyi ilk username üzerinden kontrol ettik
        if (user == null)
        { // eğer böyle bir username yoksa email olarak kontrol ettik
            user = await _userManager.FindByEmailAsync(request.EmailOrUserName);
        }

        if (user == null) // böyle bir email de yoksa böyle bir kullanıcı yoktur. hata fırlattık
            throw new Exception("Kullanıcı veya Şifre Hatalı");

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            false
        );

        if (signInResult.Succeeded)
        {
            //yetki belirleme
            Token token = _tokenHandler.CreateAccessToken(180);

            return new LoginUserSuccessCommandResponse() { Token = token };
        }

        return new LoginUserErrorCommandResponse() { Message = "Kullanıcı adı veya Şifre Hatalı" };
    }
}
