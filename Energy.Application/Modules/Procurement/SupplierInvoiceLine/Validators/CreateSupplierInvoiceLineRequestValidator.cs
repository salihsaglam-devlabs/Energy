using FluentValidation;
using Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;

namespace Energy.Application.Modules.Procurement.SupplierInvoiceLine.Validators;

/// <summary>CreateSupplierInvoiceLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateSupplierInvoiceLineRequestValidator : AbstractValidator<CreateSupplierInvoiceLineRequest>
{
    public CreateSupplierInvoiceLineRequestValidator()
    {
        RuleFor(x => x.SupplierInvoiceId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
