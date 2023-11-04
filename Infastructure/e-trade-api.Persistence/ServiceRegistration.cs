using e_trade_api.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using e_trade_api.application;
using e_trade_api.domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace e_trade_api.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration _configuration
        )
        {
            services.AddDbContext<ETradeApiDBContext>(
                options => options.UseNpgsql(_configuration["ConnectionStrings:SQLServer"]),
                ServiceLifetime.Scoped
            );
            services.AddScoped<DesignTimeDbContextFactory>();

            services
                .AddIdentity<AppUser, AppRole>(options =>
                {
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;

                    options.User.RequireUniqueEmail = true; //herkesin farklı maili olsun

                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
                })
                .AddEntityFrameworkStores<ETradeApiDBContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>();

            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();
            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            services.AddScoped<IBasketService, BasketService>();

            services.AddScoped<IOrderItemReadRepository, OrderItemReadRepository>();
            services.AddScoped<IOrderItemWriteRepository, OrderItemWriteRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ICompletedOrderReadRepository, CompletedOrderReadRepository>();
            services.AddScoped<ICompletedOrderWriteRepository, CompletedOrderWriteRepository>();

            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IEndpointReadRepository, EndpointReadRepository>();
            services.AddScoped<IEndpointWriteRepository, EndpointWriteRepository>();
            services.AddScoped<IMenuReadRepository, MenuReadRepository>();
            services.AddScoped<IMenuWriteRepository, MenuWriteRepository>();

            services.AddScoped<IAuthorizationEndpointService, AuthorizationEndpointService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IAddressReadRepository, AddressReadRepository>();
            services.AddScoped<IAddressWriteRepository, AddressWriteRepository>();

            services.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
            services.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();

            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IProductRatingReadRepository, ProductRatingReadRepository>();
            services.AddScoped<IProductRatingWriteRepository, ProductRatingWriteRepository>();

            services.AddScoped<IProductRatingService, ProductRatingService>();

            services.AddScoped<
                ITwoFactorAuthenticationReadRepository,
                TwoFactorAuthenticationReadRepository
            >();
            services.AddScoped<
                ITwoFactorAuthenticationWriteRepository,
                TwoFactorAuthenticationWriteRepository
            >();

            services.AddScoped<ITwoFactorAuthenticationService, TwoFactorAuthenticationService>();

            services.AddScoped<IImageFileReadRepository, ImageFileReadRepository>();
            services.AddScoped<IImageFileWriteRepository, ImageFileWriteRepository>();
            services.AddScoped<IImageFileService, ImageFileService>();

            services.AddScoped<ICloudflareService, CloudflareService>();
            services.AddScoped<HttpClient>();
        }
    }
}
