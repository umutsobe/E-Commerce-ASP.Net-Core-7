using MediatR;

namespace e_trade_api.application;

public class PasswordResetCommandRequest : IRequest<PasswordResetCommandResponse>
{
    public string Email { get; set; }
}
