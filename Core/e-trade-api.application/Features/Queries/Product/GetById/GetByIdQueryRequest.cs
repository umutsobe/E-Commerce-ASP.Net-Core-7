using MediatR;

namespace e_trade_api.application;

public class GetByIdQueryRequest : IRequest<GetByIdQueryResponse>
{
    public string Id { get; set; }
}
