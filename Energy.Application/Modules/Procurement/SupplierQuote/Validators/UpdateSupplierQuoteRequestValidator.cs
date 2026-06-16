using FluentValidation;
using Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;

namespace Energy.Application.Modules.Procurement.SupplierQuote.Validators;

/// <summary>UpdateSupplierQuoteRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateSupplierQuoteRequestValidator : AbstractValidator<UpdateSupplierQuoteRequest>
{
    public UpdateSupplierQuoteRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.QuoteNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
