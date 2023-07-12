using e_trade_api.domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence.Contexts
{
    public class ETradeApiDBContext : DbContext
    {
        public ETradeApiDBContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
