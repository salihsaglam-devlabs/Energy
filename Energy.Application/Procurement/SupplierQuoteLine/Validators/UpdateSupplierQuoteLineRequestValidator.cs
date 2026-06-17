using FluentValidation;
using Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Requests;

namespace Energy.Application.Procurement.SupplierQuoteLine.Validators;

/// <summary>UpdateSupplierQuoteLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateSupplierQuoteLineRequestValidator : AbstractValidator<UpdateSupplierQuoteLineRequest>
{
    public UpdateSupplierQuoteLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SupplierQuoteId).NotEmpty();
    }
}
