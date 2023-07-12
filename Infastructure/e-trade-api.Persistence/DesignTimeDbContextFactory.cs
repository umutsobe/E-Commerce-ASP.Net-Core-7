using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_trade_api.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ETradeApiDBContext>
    {
        public ETradeApiDBContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ETradeApiDBContext> dbContextOptionsBuilder = new();

            dbContextOptionsBuilder.UseSqlServer(Configuration.GetConnectionString());

            return new(dbContextOptionsBuilder.Options);
        }
    }
}
