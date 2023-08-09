using Microsoft.AspNetCore.Identity;
using MediatR;
using e_trade_api.domain;

namespace e_trade_api.application;

public class CreateUserCommandHandle
    : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    readonly UserManager<AppUser> _userManager; //repository işlevi gören usermanager
    readonly IBasketService _basketService;

    public CreateUserCommandHandle(UserManager<AppUser> userManager, IBasketService basketService)
    {
        _userManager = userManager;
        _basketService = basketService;
    }

    public async Task<CreateUserCommandResponse> Handle(
        CreateUserCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        AppUser user = new AppUser
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name,
            Email = request.Email,
            UserName = request.UserName,
        };

        IdentityResult userResult = await _userManager.CreateAsync(user, request.Password);

        bool basketResult = await _basketService.CreateBasket(user.Id.ToString());

        CreateUserCommandResponse response = new CreateUserCommandResponse();

        if (userResult.Succeeded && basketResult)
        {
            response.Succeeded = true;
            response.Message = "Kullanıcı Başarıyla Oluşturulmuştur";
        }
        else
        {
            response.Succeeded = false;
            foreach (var e in userResult.Errors)
                response.Message += $"{e.Code} - {e.Description}\n";
        }
        return response;
    }
}
