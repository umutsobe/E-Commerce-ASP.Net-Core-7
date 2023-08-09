using e_trade_api.application;
using Microsoft.AspNetCore.SignalR;

namespace e_trade_api.SignalR;

public class OrderHubService : IOrderHubService
{
    readonly IHubContext<OrderHub> _hubContext;

    public OrderHubService(IHubContext<OrderHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task OrderAddedMessageAsync(string message) =>
        await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.OrderAddedMessage, message);
}
