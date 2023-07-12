using e_trade_api.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ETradeApiDBContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString())
            );
        }
    }
}
