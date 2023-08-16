using MediatR;

namespace e_trade_api.application;

public class UpdatePasswordCommandRequest : IRequest<UpdatePasswordCommandResponse>
{
    public string UserId { get; set; }
    public string ResetToken { get; set; }
    public string Password { get; set; }
    public string PasswordRepeat { get; set; }
}
