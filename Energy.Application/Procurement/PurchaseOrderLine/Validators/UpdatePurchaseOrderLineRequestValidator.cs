using FluentValidation;
using Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Requests;

namespace Energy.Application.Procurement.PurchaseOrderLine.Validators;

/// <summary>UpdatePurchaseOrderLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdatePurchaseOrderLineRequestValidator : AbstractValidator<UpdatePurchaseOrderLineRequest>
{
    public UpdatePurchaseOrderLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PurchaseOrderId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
