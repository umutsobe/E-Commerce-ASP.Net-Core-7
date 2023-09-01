using FluentValidation;

namespace e_trade_api.application;

public class CreateUserAddressValidator : AbstractValidator<CreateUserAddress>
{
    public CreateUserAddressValidator()
    {
        RuleFor(u => u.Definition)
            .NotEmpty()
            .NotNull()
            .WithMessage("Definition boş olamaz")
            .Length(1, 50)
            .WithMessage("Definition uzunluğu maksimum 50 karakterli olmalıdır");

        RuleFor(u => u.Address)
            .NotEmpty()
            .NotNull()
            .WithMessage("Adres boş olamaz")
            .Length(1, 200)
            .WithMessage("Definition uzunluğu maksimum 200 karakterli olmalıdır");

        RuleFor(u => u.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId boş olamaz")
            .Length(1, 50)
            .WithMessage("UserId uzunluğu maksimum 50 karakterli olmalıdır");
    }
}
