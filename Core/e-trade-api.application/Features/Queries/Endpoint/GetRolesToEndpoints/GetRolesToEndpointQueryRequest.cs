using MediatR;

namespace e_trade_api.application;

public class GetRolesToEndpointQueryRequest : IRequest<GetRolesToEndpointQueryResponse>
{
    public string Code { get; set; }
    public string Menu { get; set; }
}
