using FluentValidation;
using Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;

namespace Energy.Application.Modules.Procurement.PurchaseReceipt.Validators;

/// <summary>UpdatePurchaseReceiptRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdatePurchaseReceiptRequestValidator : AbstractValidator<UpdatePurchaseReceiptRequest>
{
    public UpdatePurchaseReceiptRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.ReceiptNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
