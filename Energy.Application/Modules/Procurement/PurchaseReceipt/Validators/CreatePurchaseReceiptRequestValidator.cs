using FluentValidation;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;

namespace Energy.Application.Modules.Procurement.PurchaseReceipt.Validators;

/// <summary>CreatePurchaseReceiptRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreatePurchaseReceiptRequestValidator : AbstractValidator<CreatePurchaseReceiptRequest>
{
    public CreatePurchaseReceiptRequestValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ReceiptNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
