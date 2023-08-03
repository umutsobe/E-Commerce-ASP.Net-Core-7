using Microsoft.AspNetCore.Builder;

namespace e_trade_api.SignalR;

public static class HubRegistration
{
    public static void MapHubs(this WebApplication webApplication)
    {
        webApplication.MapHub<ProductHub>("/product-hub");
    }
}
