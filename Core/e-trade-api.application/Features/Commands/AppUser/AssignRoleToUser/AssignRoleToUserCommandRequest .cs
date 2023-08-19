using MediatR;

namespace e_trade_api.application;

public class AssignRoleToUserCommandRequest : IRequest<AssignRoleToUserCommandResponse>
{
    public string UserId { get; set; }
    public string[] Roles { get; set; }
}
