using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Requests;

namespace Energy.Application.Modules.Inventory.StockCountLine.Validators;

/// <summary>UpdateStockCountLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockCountLineRequestValidator : AbstractValidator<UpdateStockCountLineRequest>
{
    public UpdateStockCountLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.StockCountId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
