using MediatR;

namespace e_trade_api.application;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQueryRequest, GetRolesQueryResponse>
{
    readonly IRoleService _roleService;

    public GetRolesQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<GetRolesQueryResponse> Handle(
        GetRolesQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        var (datas, count) = _roleService.GetAllRoles(request.Page, request.Size);
        return new() { Datas = datas, TotalCount = count };
    }
}
