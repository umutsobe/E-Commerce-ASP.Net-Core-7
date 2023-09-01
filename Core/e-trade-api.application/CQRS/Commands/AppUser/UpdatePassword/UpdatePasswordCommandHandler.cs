using MediatR;

namespace e_trade_api.application;

public class UpdatePasswordCommandHandler
    : IRequestHandler<UpdatePasswordCommandRequest, UpdatePasswordCommandResponse>
{
    readonly IUserService _userService;

    public UpdatePasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UpdatePasswordCommandResponse> Handle(
        UpdatePasswordCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        if (!request.Password.Equals(request.PasswordRepeat))
            throw new Exception("Lütfen şifreyi birebir doğrulayınız.");

        await _userService.UpdatePasswordAsync(
            request.UserId,
            request.ResetToken,
            request.Password
        );
        return new();
    }
}
