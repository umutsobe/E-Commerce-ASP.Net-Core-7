using FluentValidation;

namespace e_trade_api.application;

public class RemoveBasketItemValidator : AbstractValidator<RemoveBasketItemCommandRequest>
{
    public RemoveBasketItemValidator()
    {
        RuleFor(b => b.BasketItemId)
            .NotEmpty()
            .NotNull()
            .WithMessage("BasketItemId boş olamaz")
            .MaximumLength(50)
            .WithMessage("BasketItemId maksimum uzunluğu 50 olmalıdır");
    }
}
