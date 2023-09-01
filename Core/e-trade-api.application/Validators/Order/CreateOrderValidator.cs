using FluentValidation;

namespace e_trade_api.application;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommandRequest>
{
    public CreateOrderValidator()
    {
        RuleFor(o => o.UserId)
            .NotEmpty()
            .NotNull()
            .WithMessage("UserId boş olamaz")
            .Length(1, 50)
            .WithMessage("UserId 50 karakterden fazla olamaz");

        RuleFor(u => u.Description)
            .NotEmpty()
            .NotNull()
            .WithMessage("Description boş olamaz")
            .Length(1, 100)
            .WithMessage("Description uzunluğu maksimum 100 karakterli olmalıdır");

        RuleFor(u => u.Address)
            .NotEmpty()
            .NotNull()
            .WithMessage("Adres boş olamaz")
            .Length(1, 200)
            .WithMessage("Definition uzunluğu maksimum 200 karakterli olmalıdır");

        RuleForEach(o => o.OrderItems)
            .Must(BeValidOrderItem)
            .WithMessage("Sipariş öğeleri geçersiz.");

        RuleFor(request => request.OrderItems)
            .Must(orderItems => orderItems != null && orderItems.Count > 0)
            .WithMessage("En az bir sipariş öğesi gereklidir.");
    }

    private bool BeValidOrderItem(CreateOrderItem orderItem)
    {
        // Fiyat veya miktar negatifse hata döndür
        if (orderItem == null || orderItem.Price <= 0 || orderItem.Quantity < 1)
        {
            return false;
        }
        return true;
    }
}
