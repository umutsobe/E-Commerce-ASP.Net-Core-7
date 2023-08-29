using e_trade_api.domain;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace e_trade_api.application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceRegistration));
    }
}
