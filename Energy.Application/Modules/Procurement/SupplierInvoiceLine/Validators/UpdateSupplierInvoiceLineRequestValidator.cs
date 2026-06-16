using FluentValidation;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Validators;

/// <summary>UpdateSupplierInvoiceLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateSupplierInvoiceLineRequestValidator : AbstractValidator<UpdateSupplierInvoiceLineRequest>
{
    public UpdateSupplierInvoiceLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SupplierInvoiceId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
