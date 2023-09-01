using MediatR;

namespace e_trade_api.application;

public class CompleteOrderCommandRequest : IRequest<CompleteOrderCommandResponse>
{
    public string Id { get; set; }
}
