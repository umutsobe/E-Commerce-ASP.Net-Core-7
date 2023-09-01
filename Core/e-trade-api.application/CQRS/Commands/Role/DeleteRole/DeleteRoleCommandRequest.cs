using MediatR;

namespace e_trade_api.application;

public class DeleteRoleCommandRequest : IRequest<DeleteRoleCommandResponse>
{
    public string Id { get; set; }
}
