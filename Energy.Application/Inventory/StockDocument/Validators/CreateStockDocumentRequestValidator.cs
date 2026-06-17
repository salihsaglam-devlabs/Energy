using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockDocument.Requests;

namespace Energy.Application.Inventory.StockDocument.Validators;

/// <summary>CreateStockDocumentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockDocumentRequestValidator : AbstractValidator<CreateStockDocumentRequest>
{
    public CreateStockDocumentRequestValidator()
    {
        RuleFor(x => x.DocumentTypeId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.DocumentNo).NotEmpty();
    }
}
