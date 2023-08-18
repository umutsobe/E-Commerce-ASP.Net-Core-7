using MediatR;

namespace e_trade_api.application;

public class UpdateRoleCommandRequest : IRequest<UpdateRoleCommandResponse>
{
    public string Id { get; set; }
    public string Name { get; set; }
}
