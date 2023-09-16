using System.ComponentModel;
using FluentValidation;

namespace e_trade_api.application;

public class UpdateBasketItemQuantityValidator : AbstractValidator<UpdateBasketItemRequestDTO>
{
    public UpdateBasketItemQuantityValidator()
    {
        RuleFor(o => o.Quantity)
            .NotEmpty()
            .NotNull()
            .WithMessage("Quantity değeri boş olamaz")
            .Must(o => o > 0)
            .WithMessage("Quantity Değeri sıfırdan küçük olamaz");

        RuleFor(o => o.BasketItemId)
            .NotEmpty()
            .NotNull()
            .WithMessage("BasketItemId değeri boş olamaz")
            .Length(1, 50)
            .WithMessage("BasketItemId 50 karakterden fazla olamaz");
    }
}
