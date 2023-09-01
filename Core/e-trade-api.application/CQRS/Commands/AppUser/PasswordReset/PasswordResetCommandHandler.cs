using MediatR;

namespace e_trade_api.application;

public class PasswordResetCommandHandler
    : IRequestHandler<PasswordResetCommandRequest, PasswordResetCommandResponse>
{
    readonly IAuthService _authService;

    public PasswordResetCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<PasswordResetCommandResponse> Handle(
        PasswordResetCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _authService.PasswordResetAsync(request.Email);
        return new();
    }
}
