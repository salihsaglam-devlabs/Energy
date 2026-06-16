using FluentValidation;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;

namespace Energy.Application.Modules.Procurement.SupplierInvoice.Validators;

/// <summary>CreateSupplierInvoiceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateSupplierInvoiceRequestValidator : AbstractValidator<CreateSupplierInvoiceRequest>
{
    public CreateSupplierInvoiceRequestValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.InvoiceNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
