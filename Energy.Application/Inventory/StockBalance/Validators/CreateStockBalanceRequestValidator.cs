using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockBalance.Requests;

namespace Energy.Application.Inventory.StockBalance.Validators;

/// <summary>CreateStockBalanceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockBalanceRequestValidator : AbstractValidator<CreateStockBalanceRequest>
{
    public CreateStockBalanceRequestValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
