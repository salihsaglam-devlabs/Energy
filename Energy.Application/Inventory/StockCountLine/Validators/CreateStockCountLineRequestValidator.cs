using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;

namespace Energy.Application.Inventory.StockCountLine.Validators;

/// <summary>CreateStockCountLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockCountLineRequestValidator : AbstractValidator<CreateStockCountLineRequest>
{
    public CreateStockCountLineRequestValidator()
    {
        RuleFor(x => x.StockCountId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
