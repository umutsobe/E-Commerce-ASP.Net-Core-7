using MediatR;

namespace e_trade_api.application;

public class GetAllOrdersQueryHandler
    : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
{
    readonly IOrderService _orderService;

    public GetAllOrdersQueryHandler(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<GetAllOrdersQueryResponse> Handle(
        GetAllOrdersQueryRequest request,
        CancellationToken cancellationToken
    )
    {
        var data = await _orderService.GetAllOrdersAsync(request.Page, request.Size);

        return new() { TotalOrderCount = data.TotalOrderCount, Orders = data.Orders };
    }
}