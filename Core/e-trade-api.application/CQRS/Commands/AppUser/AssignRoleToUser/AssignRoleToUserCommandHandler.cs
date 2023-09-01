using MediatR;

namespace e_trade_api.application;

public class AssignRoleToUserCommandHandler
    : IRequestHandler<AssignRoleToUserCommandRequest, AssignRoleToUserCommandResponse>
{
    readonly IUserService _userService;

    public AssignRoleToUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<AssignRoleToUserCommandResponse> Handle(
        AssignRoleToUserCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _userService.AssignRoleToUserAsnyc(request.UserId, request.Roles);
        return new();
    }
}
