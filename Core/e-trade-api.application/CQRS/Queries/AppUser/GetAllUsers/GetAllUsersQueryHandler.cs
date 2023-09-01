using MediatR;

namespace e_trade_api.application;

public class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQueryRequest, GetAllUsersQueryResponse>
{
    readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<GetAllUsersQueryResponse> Handle(
        GetAllUsersQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userService.GetAllUsersAsync(request.Page, request.Size);
        return new() { Users = users, TotalUsersCount = _userService.TotalUsersCount };
    }
}
