using MediatR;

namespace e_trade_api.application;

public class GetAllUsersQueryRequest : IRequest<GetAllUsersQueryResponse>
{
    public int Page { get; set; }
    public int Size { get; set; }
}
