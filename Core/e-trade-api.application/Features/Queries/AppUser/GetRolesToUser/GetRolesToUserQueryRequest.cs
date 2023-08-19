using MediatR;

namespace e_trade_api.application;

public class GetRolesToUserQueryRequest : IRequest<GetRolesToUserQueryResponse>
{
    public string UserId { get; set; }
}
