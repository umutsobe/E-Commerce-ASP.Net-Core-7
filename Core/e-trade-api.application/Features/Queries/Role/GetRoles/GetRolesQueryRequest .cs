using MediatR;

namespace e_trade_api.application;

public class GetRolesQueryRequest : IRequest<GetRolesQueryResponse>
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
