using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Requests;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Validators;

/// <summary>UpdateStockDocumentTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockDocumentTypeRequestValidator : AbstractValidator<UpdateStockDocumentTypeRequest>
{
    public UpdateStockDocumentTypeRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Direction).NotEmpty();
    }
}
