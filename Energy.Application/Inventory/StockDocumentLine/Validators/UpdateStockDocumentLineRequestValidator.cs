using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Requests;

namespace Energy.Application.Inventory.StockDocumentLine.Validators;

/// <summary>UpdateStockDocumentLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockDocumentLineRequestValidator : AbstractValidator<UpdateStockDocumentLineRequest>
{
    public UpdateStockDocumentLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.StockDocumentId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.UnitOfMeasureId).NotEmpty();
    }
}
