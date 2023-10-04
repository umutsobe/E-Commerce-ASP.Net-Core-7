using e_trade_api.application;
using e_trade_api.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETradeApiDBContext>
{
    readonly IConfiguration _configuration;

    public DesignTimeDbContextFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ETradeApiDBContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<ETradeApiDBContext> dbContextOptionsBuilder = new();

        dbContextOptionsBuilder.UseSqlServer(_configuration.GetConnectionString("SQLServer"));

        return new(dbContextOptionsBuilder.Options);
    }
}
