using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockCount.Requests;

namespace Energy.Application.Inventory.StockCount.Validators;

/// <summary>UpdateStockCountRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockCountRequestValidator : AbstractValidator<UpdateStockCountRequest>
{
    public UpdateStockCountRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.CountNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
