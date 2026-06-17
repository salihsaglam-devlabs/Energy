using FluentValidation;
using Energy.Shared.Models.V1.Finance.Collection.Requests;

namespace Energy.Application.Finance.Collection.Validators;

/// <summary>CreateCollectionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateCollectionRequestValidator : AbstractValidator<CreateCollectionRequest>
{
    public CreateCollectionRequestValidator()
    {
        RuleFor(x => x.PartnerId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.CollectionNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
