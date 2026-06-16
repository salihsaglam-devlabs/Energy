using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;

namespace Energy.Application.Inventory.StockTransaction.Validators;

/// <summary>UpdateStockTransactionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockTransactionRequestValidator : AbstractValidator<UpdateStockTransactionRequest>
{
    public UpdateStockTransactionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.StockDocumentId).NotEmpty();
        RuleFor(x => x.StockDocumentLineId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
