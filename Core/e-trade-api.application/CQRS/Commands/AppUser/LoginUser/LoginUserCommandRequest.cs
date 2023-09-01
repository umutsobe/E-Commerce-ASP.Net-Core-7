using MediatR;

namespace e_trade_api.application;

public class LoginUserCommandRequest : IRequest<LoginUserCommandResponse>
{
    public string EmailOrUserName { get; set; }
    public string Password { get; set; }
}
