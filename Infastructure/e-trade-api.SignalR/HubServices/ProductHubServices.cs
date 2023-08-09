using e_trade_api.application;
using Microsoft.AspNetCore.SignalR;

namespace e_trade_api.SignalR;

public class ProductHubServices : IProductHubServices
{
    readonly IHubContext<ProductHub> _hubContext;

    public ProductHubServices(IHubContext<ProductHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task ProductAddedMessageAsync(string message)
    {
        await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ProductAddedMessage, message);
    }
}
