using e_trade_api.application;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace e_trade_api.API;

public static class ConfigureValidations
{
    public static void AddValidationsServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();

        services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateBasketItemQuantityValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateUserAddressValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserEmailValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateUserNameValidator>();
        services.AddValidatorsFromAssemblyContaining<AddItemToBasketValidator>();
        services.AddValidatorsFromAssemblyContaining<RemoveBasketItemValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateOrderValidator>();
    }
}
