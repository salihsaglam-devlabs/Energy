using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;

namespace Energy.Application.Modules.Inventory.StockCount.Validators;

/// <summary>CreateStockCountRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockCountRequestValidator : AbstractValidator<CreateStockCountRequest>
{
    public CreateStockCountRequestValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.CountNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
