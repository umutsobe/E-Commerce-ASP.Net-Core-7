using Microsoft.Extensions.DependencyInjection;
using e_trade_api.application;

namespace e_trade_api.Infastructure;

public static class ServiceRegistiration
{
    public static void AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
    }
}
