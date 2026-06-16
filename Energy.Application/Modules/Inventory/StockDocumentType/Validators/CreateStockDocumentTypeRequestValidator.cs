using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Validators;

/// <summary>CreateStockDocumentTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateStockDocumentTypeRequestValidator : AbstractValidator<CreateStockDocumentTypeRequest>
{
    public CreateStockDocumentTypeRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Direction).NotEmpty();
    }
}
