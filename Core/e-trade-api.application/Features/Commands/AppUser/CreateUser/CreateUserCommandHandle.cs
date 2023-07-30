using Microsoft.AspNetCore.Identity;
using MediatR;
using e_trade_api.domain;

namespace e_trade_api.application;

public class CreateUserCommandHandle
    : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    readonly UserManager<AppUser> _userManager;

    public CreateUserCommandHandle(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserCommandResponse> Handle(
        CreateUserCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        IdentityResult result = await _userManager.CreateAsync(
            new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name,
                Email = request.Email,
                UserName = request.UserName
            },
            request.Password
        );

        CreateUserCommandResponse response = new CreateUserCommandResponse();

        if (result.Succeeded)
        {
            response.Succeeded = true;
            response.Message = "Kullanıcı Başarıyla Oluşturulmuştur";
        }
        else
        {
            response.Succeeded = false;
            foreach (var e in result.Errors)
                response.Message += $"{e.Code} - {e.Description}\n";
        }
        return response;
    }
}
