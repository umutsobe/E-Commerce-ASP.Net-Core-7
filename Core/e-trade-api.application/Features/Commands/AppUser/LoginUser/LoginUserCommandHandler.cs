using e_trade_api.domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace e_trade_api.application;

public class LoginUserCommandHandler
    : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
{
    readonly IUserService _userService;

    public LoginUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<LoginUserCommandResponse> Handle(
        LoginUserCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        return await _userService.LoginUser(
            new() { EmailOrUserName = request.EmailOrUserName, Password = request.Password, }
        );
    }
}
