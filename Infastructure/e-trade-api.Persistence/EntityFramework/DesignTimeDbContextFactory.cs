using e_trade_api.application;
using e_trade_api.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace e_trade_api.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETradeApiDBContext>
{
    public ETradeApiDBContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ETradeApiDBContext> dbContextOptionsBuilder = new();

        dbContextOptionsBuilder.UseSqlServer(MyConfigurationManager.GetConnectionString());

        return new(dbContextOptionsBuilder.Options);
    }
}
