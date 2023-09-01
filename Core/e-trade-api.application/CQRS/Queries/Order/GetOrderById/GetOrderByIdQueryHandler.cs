using MediatR;

namespace e_trade_api.application;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
{
    readonly IOrderService _orderService;

    public GetOrderByIdQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<GetOrderByIdQueryResponse> Handle(
        GetOrderByIdQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        var data = await _orderService.GetOrderByIdAsync(request.Id);

        return new()
        {
            Id = data.Id,
            OrderCode = data.OrderCode,
            Address = data.Address,
            OrderItems = data.OrderItems,
            CreatedDate = data.CreatedDate,
            Description = data.Description
        };
    }
}
