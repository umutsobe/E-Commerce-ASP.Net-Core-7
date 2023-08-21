using System.Security.Claims;
using System.Text;
using e_trade_api.API;
using e_trade_api.application;
using e_trade_api.Infastructure;
using e_trade_api.Persistence;
using e_trade_api.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureService();
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();
builder.Services.AddControllers();

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        "Admin",
        options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = builder.Configuration["Token:Audience"],
                ValidIssuer = builder.Configuration["Token:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])
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
                        "http://localhost:4200",
                        "https://localhost:4200",
                        "https://mini-e-trade.azurewebsites.net"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
        )
);

builder.Services.AddControllers();
builder.Services.AddValidationsServices();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<RolePermissionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHubs();

app.Run();
