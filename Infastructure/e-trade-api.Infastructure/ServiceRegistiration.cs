using Microsoft.Extensions.DependencyInjection;
using e_trade_api.application;

namespace e_trade_api.Infastructure;

public static class ServiceRegistiration
{
    public static void AddInfrastructureService(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();
    }

    public static void AddStorage<T>(this IServiceCollection services)
        where T : class, IStorage // normalde sadece IStorage demeliydik. ama interface olduğu için class da dedik. yani istorage'dan türeyen bir class oraya gelsin diyoruz.
    {
        services.AddScoped<IStorage, T>();
    }
}
