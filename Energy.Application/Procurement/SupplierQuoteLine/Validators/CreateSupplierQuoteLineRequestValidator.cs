using FluentValidation;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;

namespace Energy.Application.Procurement.SupplierQuoteLine.Validators;

/// <summary>CreateSupplierQuoteLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateSupplierQuoteLineRequestValidator : AbstractValidator<CreateSupplierQuoteLineRequest>
{
    public CreateSupplierQuoteLineRequestValidator()
    {
        RuleFor(x => x.SupplierQuoteId).NotEmpty();
    }
}
