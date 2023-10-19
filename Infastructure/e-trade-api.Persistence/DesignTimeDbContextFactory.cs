using e_trade_api.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETradeApiDBContext>
{
    //public DesignTimeDbContextFactory() { } //System.MissingMethodException: Cannot dynamically create an instance of type 'e_trade_api.Persistence.DesignTimeDbContextFactory'. Reason: No parameterless constructor defined. System.MissingMethodException: Cannot dynamically create an instance of type 'e_trade_api.Persistence.DesignTimeDbContextFactory'. Reason: No parameterless constructor defined.

    //bu hatanın çözümü constructure eklemek

    // public DesignTimeDbContextFactory(IConfiguration configuration)
    // {
    //     _configuration = configuration;
    // }

    private readonly IConfiguration _configuration;

    public DesignTimeDbContextFactory() //hata almamam için bu parametresiz olmalı yoksa yukarıdaki hatayı alırım
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets("cdc4cfa8-e8bd-4ffb-87d7-d0cd4294f54c") //bu secret persistence projesinin user secretı
            .AddEnvironmentVariables();

        _configuration = builder.Build();
    }

    public ETradeApiDBContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().Build();

        DbContextOptionsBuilder<ETradeApiDBContext> dbContextOptionsBuilder = new();

        dbContextOptionsBuilder.UseNpgsql(_configuration.GetConnectionString("SQLServer"));

        return new(dbContextOptionsBuilder.Options);
    }
}
