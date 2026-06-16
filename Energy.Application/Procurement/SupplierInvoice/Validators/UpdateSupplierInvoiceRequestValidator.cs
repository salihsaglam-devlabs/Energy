using FluentValidation;
using Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;

namespace Energy.Application.Procurement.SupplierInvoice.Validators;

/// <summary>UpdateSupplierInvoiceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateSupplierInvoiceRequestValidator : AbstractValidator<UpdateSupplierInvoiceRequest>
{
    public UpdateSupplierInvoiceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.InvoiceNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
