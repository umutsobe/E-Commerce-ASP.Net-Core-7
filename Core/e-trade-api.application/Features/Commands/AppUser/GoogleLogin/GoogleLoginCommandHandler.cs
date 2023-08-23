using e_trade_api.domain;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.application;

public class GoogleLoginCommandHandler
    : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
{
    readonly UserManager<AppUser> _userManager;
    readonly ITokenHandler _tokenHandler;
    readonly IBasketService _basketService;

    public GoogleLoginCommandHandler(
        UserManager<AppUser> userManager,
        ITokenHandler tokenHandler,
        IBasketService basketService
    )
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
        _basketService = basketService;
    }

    public async Task<GoogleLoginCommandResponse> Handle(
        GoogleLoginCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>
                {
                    "719960856381-8eaoopcgmmkjn2nv91bklf4utlqkc62s.apps.googleusercontent.com"
                }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

            var user = await _userManager.FindByLoginAsync(request.Provider, payload.Subject);
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
                        Name = payload.Name
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
                new UserLoginInfo(request.Provider, payload.Subject, request.Provider)
            );

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            
            if(userRoles.Count == 0)
            await _userManager.AddToRoleAsync(user,"normalUser");

            Token token = await _tokenHandler.CreateAccessToken(180, userId.ToString());

            return new GoogleLoginCommandResponse { Token = token };
        }
        catch (Exception ex)
        {
            // Log the error and handle it appropriately.
            // Return a meaningful error response to the client.
            throw new Exception("External authentication error.", ex);
        }
    }
}
