using FluentValidation;

namespace e_trade_api.application;

public class AddItemToBasketValidator : AbstractValidator<CreateBasketItemRequestDTO>
{
    public AddItemToBasketValidator()
    {
        RuleFor(b => b.BasketId)
            .NotEmpty()
            .NotNull()
            .WithMessage("BasketId boş olamaz")
            .MaximumLength(50)
            .WithMessage("BasketId maksimum uzunluğu 50 olmalıdır");

        RuleFor(b => b.ProductId)
            .NotEmpty()
            .NotNull()
            .WithMessage("ProductId boş olamaz")
            .MaximumLength(50)
            .WithMessage("ProductId maksimum uzunluğu 50 olmalıdır");

        RuleFor(b => b.Quantity)
            .NotEmpty()
            .NotNull()
            .WithMessage("Quantity boş olamaz")
            .Must(b => b > 0)
            .WithMessage("Quantity sıfırdan küçük olamaz");
    }
}
