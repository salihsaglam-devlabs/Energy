using FluentValidation;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Validators;

/// <summary>CreatePurchaseReceiptLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreatePurchaseReceiptLineRequestValidator : AbstractValidator<CreatePurchaseReceiptLineRequest>
{
    public CreatePurchaseReceiptLineRequestValidator()
    {
        RuleFor(x => x.PurchaseReceiptId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
