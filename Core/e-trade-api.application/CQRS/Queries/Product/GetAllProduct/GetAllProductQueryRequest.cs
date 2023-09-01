using MediatR;

namespace e_trade_api.application;

public class GetAllProductQueryRequest : IRequest<GetAllProductQueryResponse>
{
    // public Pagination pagination { get; set; }

    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;

}
