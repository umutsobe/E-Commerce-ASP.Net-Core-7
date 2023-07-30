using e_trade_api.API;
using e_trade_api.application;
using e_trade_api.Infastructure;
using e_trade_api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServices();
builder.Services.AddInfrastructureService();
builder.Services.AddApplicationServices();
builder.Services.AddControllers();

// builder.Services.AddStorage<LocalStorage>();

builder.Services.AddStorage<AzureStorage>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddCors(
    options =>
        options.AddDefaultPolicy(
            policy =>
                policy
                    // .WithOrigins("http://localhost:4200/", "https://localhost:4200/") //sadece izin verilen url'ye response sağlansın. origin= kaynak
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
        )
);
builder.Services.AddControllers();
builder.Services.AddValidationsServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
