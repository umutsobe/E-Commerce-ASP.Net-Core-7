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
    }
}

//bu dosyayı ben yaptım. fluent validation güncellendiği için eski şekilde yapamıyorduk.
// oku https://github.com/FluentValidation/FluentValidation/issues/1963
