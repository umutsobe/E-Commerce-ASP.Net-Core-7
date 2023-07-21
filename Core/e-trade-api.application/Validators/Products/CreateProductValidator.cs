using FluentValidation;

namespace e_trade_api.application;

public class CreateProductValidator : AbstractValidator<VM_Create_Product>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .NotNull()
            .WithMessage("Lütfen Ürün adını boş geçmeyiniz")
            .MaximumLength(150)
            .WithMessage("Ürün ismi 150 karakterden az olmalıdır. Lütfen daha kısa isim giriniz");

        RuleFor(p => p.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Lütfen Ürün fiyat bilgisini boş geçmeyiniz")
            .Must(p => p >= 0)
            .WithMessage("Fiyat bilgisi negatif olamaz");

        RuleFor(p => p.Stock)
            .NotEmpty()
            .NotNull()
            .WithMessage("Lütfen Ürün stok bilgisini boş geçmeyiniz")
            .Must(p => p >= 0)
            .WithMessage("Stok bilgisi negatif olamaz. Geçerli stok bilgisi giriniz.");
    }
}
