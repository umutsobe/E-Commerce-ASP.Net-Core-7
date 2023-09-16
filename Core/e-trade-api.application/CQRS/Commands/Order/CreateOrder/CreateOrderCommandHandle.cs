using MediatR;

namespace e_trade_api.application;

public class CreateOrderCommandHandle
    : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
{
    readonly IOrderService _orderService;
    readonly IOrderHubService _orderHubService;

    public CreateOrderCommandHandle(IOrderService orderService, IOrderHubService orderHubService)
    {
        _orderService = orderService;
        _orderHubService = orderHubService;
    }

    public async Task<CreateOrderCommandResponse> Handle(
        CreateOrderCommandRequest request,
        CancellationToken cancellationToken
    )
    {
        CreateOrderResponseDTO response = await _orderService.CreateOrderAsync(
            new()
            {
                UserId = request.UserId,
                Address = request.Address,
                Description = request.Description,
                OrderItems = request.OrderItems
            }
        );

        await _orderHubService.OrderAddedMessageAsync("Yeni bir sipariş geldi");

        return new()
        {
            Succeeded = response.Succeeded,
            Message = response.Message,
            OrderCode = response.OrderCode,
            OrderId = response.OrderId
        };
    }
}
