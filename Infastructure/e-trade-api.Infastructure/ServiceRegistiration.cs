using Microsoft.Extensions.DependencyInjection;
using e_trade_api.application;
using e_trade_api.Infrastructure;

namespace e_trade_api.Infastructure;

public static class ServiceRegistiration
{
    public static void AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();
        services.AddScoped<ITokenHandler, TokenHandler>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IApplicationService, ApplicationService>();
    }

    public static void AddStorage<T>(this IServiceCollection services)
        where T : class, IStorageService //T sadece IStorageService'ten türeyen bir class olabilir. Şimdilik azure storage. daha sonra başkaları da eklenebilir.
    {
        services.AddScoped<IStorageService, AzureStorage>();
    }
}
