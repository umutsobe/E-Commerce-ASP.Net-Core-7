using MediatR;

namespace e_trade_api.application;

public class GetAllOrdersQueryRequest : IRequest<GetAllOrdersQueryResponse>
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}
