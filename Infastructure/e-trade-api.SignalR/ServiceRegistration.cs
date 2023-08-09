using e_trade_api.application;
using Microsoft.Extensions.DependencyInjection;

namespace e_trade_api.SignalR;

public static class ServiceRegistration
{
    public static void AddSignalRServices(this IServiceCollection services)
    {
        services.AddTransient<IProductHubServices, ProductHubServices>();
        services.AddTransient<IOrderHubService, OrderHubService>();
        services.AddSignalR();
    }
}
