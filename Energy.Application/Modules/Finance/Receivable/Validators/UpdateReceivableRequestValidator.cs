using FluentValidation;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;

namespace Energy.Application.Modules.Finance.Receivable.Validators;

/// <summary>UpdateReceivableRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateReceivableRequestValidator : AbstractValidator<UpdateReceivableRequest>
{
    public UpdateReceivableRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PartnerId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
