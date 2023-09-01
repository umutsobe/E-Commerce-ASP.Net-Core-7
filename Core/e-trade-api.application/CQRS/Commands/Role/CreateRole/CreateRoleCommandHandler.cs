using MediatR;

namespace e_trade_api.application;

public class CreateRoleCommandHandler
    : IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
{
    readonly IRoleService _roleService;

    public CreateRoleCommandHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<CreateRoleCommandResponse> Handle(
        CreateRoleCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _roleService.CreateRole(request.Name);
        return new() { Succeeded = result };
    }
}
