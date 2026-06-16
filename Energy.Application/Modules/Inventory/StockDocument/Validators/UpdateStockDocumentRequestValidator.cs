using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;

namespace Energy.Application.Modules.Inventory.StockDocument.Validators;

/// <summary>UpdateStockDocumentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockDocumentRequestValidator : AbstractValidator<UpdateStockDocumentRequest>
{
    public UpdateStockDocumentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DocumentTypeId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.DocumentNo).NotEmpty();
    }
}
