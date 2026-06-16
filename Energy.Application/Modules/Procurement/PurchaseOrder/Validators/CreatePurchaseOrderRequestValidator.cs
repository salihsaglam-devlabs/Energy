using FluentValidation;
using Energy.Shared.Models.V1.Procurement.PurchaseOrder.Requests;

namespace Energy.Application.Modules.Procurement.PurchaseOrder.Validators;

/// <summary>CreatePurchaseOrderRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreatePurchaseOrderRequestValidator : AbstractValidator<CreatePurchaseOrderRequest>
{
    public CreatePurchaseOrderRequestValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.OrderNo).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
