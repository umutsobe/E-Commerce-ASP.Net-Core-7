using MediatR;

namespace e_trade_api.application;

public class CreateUserCommandHandle
    : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
{
    readonly IUserService _userService;

    public CreateUserCommandHandle(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<CreateUserCommandResponse> Handle(
        CreateUserCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateUserResponseDTO responseDTO = await _userService.CreateUser(
            new()
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password,
                RepeatPassword = request.RepeatPassword,
                UserName = request.UserName,
            }
        );

        return new()
        {
            Message = responseDTO.Message,
            Succeeded = responseDTO.Succeeded,
            UserId = responseDTO.UserId
        };
    }
}
