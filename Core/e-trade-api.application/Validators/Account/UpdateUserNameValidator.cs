using FluentValidation;

namespace e_trade_api.application;

public class UpdateUserNameValidator : AbstractValidator<UserNameUpdateDTO>
{
    public UpdateUserNameValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Name boş olamaz")
            .Length(3, 100)
            .WithMessage("Name uzunluğu 3-100 arasında olmalıdır");

        RuleFor(u => u.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId boş olamaz")
            .Length(1, 50)
            .WithMessage("UserId uzunluğu maksimum 50 karakterli olmalıdır");
    }
}
