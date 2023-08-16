using MediatR;

namespace e_trade_api.application;

public class VerifyResetTokenCommandRequest : IRequest<VerifyResetTokenCommandResponse>
{
    public string ResetToken { get; set; }
    public string UserId { get; set; }
}
