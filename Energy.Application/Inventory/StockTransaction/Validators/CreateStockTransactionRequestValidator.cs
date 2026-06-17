using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Requests;

namespace Energy.Application.Inventory.StockTransaction.Validators;

/// <summary>CreateStockTransactionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockTransactionRequestValidator : AbstractValidator<CreateStockTransactionRequest>
{
    public CreateStockTransactionRequestValidator()
    {
        RuleFor(x => x.StockDocumentId).NotEmpty();
        RuleFor(x => x.StockDocumentLineId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
