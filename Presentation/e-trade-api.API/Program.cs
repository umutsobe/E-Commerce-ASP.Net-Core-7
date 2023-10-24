using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using e_trade_api.API;
using e_trade_api.application;
using e_trade_api.Infastructure;
using e_trade_api.Persistence;
using e_trade_api.Persistence.Contexts;
using e_trade_api.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7041); // bütün domainlerden 7041'i aç
});

builder.Services.AddHttpContextAccessor(); // clienttan gelen istekteki bilgilere erişmemizi sağlayan servis
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureService();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();
builder.Services.AddControllers();

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add<RolePermissionFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddValidationsServices();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        "Auth",
        options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = builder.Configuration.GetValue<string>("Token:Audience"),
                ValidIssuer = builder.Configuration.GetValue<string>("Token:Issuer"),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration.GetValue<string>("Token:SecurityKey")
                    )
                ),
                //LifetimeValidator'ı neden oluşturduk. çünkü authorize olduktan sonra expiration date geçse dahi admin panelinde sayfayı yenilemeden route veya http istekleri yaptığımızda bu istekler karşılanıyordu. onun için expired olduktan sonra client artık backende erişemeyecek 401 authorize hatası alacak
                LifetimeValidator = (notBefore, expires, securityToken, validationParameters) =>
                    expires != null ? expires > DateTime.UtcNow : false,
                NameClaimType = ClaimTypes.Name,
            };
        }
    );

builder.Services.AddCors(
    options =>
        options.AddDefaultPolicy(
            policy =>
                policy
                    .WithOrigins(
                        builder.Configuration.GetValue<string>("AngularClientUrl"),
                        "http://localhost:4200"
                    )
                    // .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
        )
);

var app = builder.Build();

// Entity Framework Core bekleyen migrations varsa database update işlemi
if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ETradeApiDBContext>();
        dbContext.Database.Migrate(); //works
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHubs();

app.Run();
