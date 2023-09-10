using e_trade_api.domain;
using e_trade_api.domain.Entities;
using e_trade_api.domain.Entities.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace e_trade_api.Persistence.Contexts
{
    public class ETradeApiDBContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public ETradeApiDBContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ProductImageFile> ProductImageFiles { get; set; }
        public DbSet<InvoiceFile> InvoiceFiles { get; set; }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }

        public DbSet<CompletedOrder> CompletedOrders { get; set; }
        public DbSet<Endpoint> Endpoints { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Adress> Adresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductRating> ProductRatings { get; set; }
        public DbSet<TwoFactorAuthentication> TwoFactorAuthentications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //migrationda bir şeyi değiştirmedi
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>().HasIndex(o => o.OrderCode).IsUnique();

            modelBuilder
                .Entity<Order>()
                .HasOne(o => o.CompletedOrder)
                .WithOne(c => c.Order)
                .HasForeignKey<CompletedOrder>(c => c.OrderId);

            modelBuilder
                .Entity<Product>()
                .HasMany(p => p.Categories) // Bir ürünün birden fazla kategorisi olabilir.
                .WithMany(c => c.Products) // Bir kategorinin birden fazla ürünü olabilir.
                .UsingEntity(j => j.ToTable("ProductCategory")); // Ara tablo adını belirtiyoruz.

            modelBuilder
                .Entity<ProductImageFile>()
                .HasOne(pif => pif.Product)
                .WithMany(p => p.ProductImageFiles)
                .HasForeignKey(pif => pif.ProductId);
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                if (data.State == EntityState.Added)
                {
                    data.Entity.CreatedDate = DateTime.UtcNow;
                }
                if (data.State == EntityState.Modified)
                {
                    data.Entity.UpdatedDate = DateTime.UtcNow;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
