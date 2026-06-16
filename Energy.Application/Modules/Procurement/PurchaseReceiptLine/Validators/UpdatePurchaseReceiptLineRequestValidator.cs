using FluentValidation;
using Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;

namespace Energy.Application.Modules.Procurement.PurchaseReceiptLine.Validators;

/// <summary>UpdatePurchaseReceiptLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdatePurchaseReceiptLineRequestValidator : AbstractValidator<UpdatePurchaseReceiptLineRequest>
{
    public UpdatePurchaseReceiptLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PurchaseReceiptId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
