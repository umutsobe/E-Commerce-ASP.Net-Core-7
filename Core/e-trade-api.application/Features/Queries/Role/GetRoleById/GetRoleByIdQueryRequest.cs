using MediatR;

namespace e_trade_api.application;

public class GetRoleByIdQueryRequest : IRequest<GetRoleByIdQueryResponse>
{
    public string Id { get; set; }
}
