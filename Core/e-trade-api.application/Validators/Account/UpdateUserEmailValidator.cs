using FluentValidation;

namespace e_trade_api.application;

public class UpdateUserEmailValidator : AbstractValidator<UseEmailUpdateDTO>
{
    public UpdateUserEmailValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email alanı boş olamaz.")
            .EmailAddress()
            .WithMessage("Email Formatı düzgün olmalıdır");

        RuleFor(u => u.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email alanı boş olamaz.")
            .Length(1, 50)
            .WithMessage("UserId uzunluğu maksimum 50 karakterli olmalıdır");
    }
}
