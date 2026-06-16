using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Validators;

/// <summary>CreateStockDocumentLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockDocumentLineRequestValidator : AbstractValidator<CreateStockDocumentLineRequest>
{
    public CreateStockDocumentLineRequestValidator()
    {
        RuleFor(x => x.StockDocumentId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.UnitOfMeasureId).NotEmpty();
    }
}
