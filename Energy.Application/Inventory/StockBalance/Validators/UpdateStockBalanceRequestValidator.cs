using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;

namespace Energy.Application.Inventory.StockBalance.Validators;

/// <summary>UpdateStockBalanceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockBalanceRequestValidator : AbstractValidator<UpdateStockBalanceRequest>
{
    public UpdateStockBalanceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
