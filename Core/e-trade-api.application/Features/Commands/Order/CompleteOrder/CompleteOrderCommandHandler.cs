using MediatR;

namespace e_trade_api.application;

public class CompleteOrderCommandHandler
    : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
{
    readonly IOrderService _orderService;

    public CompleteOrderCommandHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<CompleteOrderCommandResponse> Handle(
        CompleteOrderCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        await _orderService.CompleteOrder(request.Id);
        return new();
    }
}
