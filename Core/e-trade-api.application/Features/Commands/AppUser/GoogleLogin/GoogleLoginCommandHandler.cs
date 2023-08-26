using e_trade_api.domain;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.application;

public class GoogleLoginCommandHandler
    : IRequestHandler<GoogleLoginCommandRequest, GoogleLoginCommandResponse>
{
    readonly IUserService _userService;

    public GoogleLoginCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<GoogleLoginCommandResponse> Handle(
        GoogleLoginCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        Token token = await _userService.GoogleLogin(
            new()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Id = request.Id,
                IdToken = request.IdToken,
                Name = request.Name,
                PhotoUrl = request.PhotoUrl,
                Provider = request.Provider,
            }
        );

        return new() { Token = token, };
    }
}
