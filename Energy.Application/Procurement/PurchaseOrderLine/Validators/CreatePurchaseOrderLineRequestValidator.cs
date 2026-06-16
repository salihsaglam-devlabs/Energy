using FluentValidation;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;

namespace Energy.Application.Procurement.PurchaseOrderLine.Validators;

/// <summary>CreatePurchaseOrderLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreatePurchaseOrderLineRequestValidator : AbstractValidator<CreatePurchaseOrderLineRequest>
{
    public CreatePurchaseOrderLineRequestValidator()
    {
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
