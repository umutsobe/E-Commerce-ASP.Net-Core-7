using MediatR;

namespace e_trade_api.application;

public class GetProductImageQueryRequest : IRequest<List<GetProductImageQueryResponse>>
{
    public string Id { get; set; }
}
