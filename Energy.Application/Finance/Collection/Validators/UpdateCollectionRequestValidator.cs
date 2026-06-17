using FluentValidation;
using Energy.Shared.Models.V1.Finance.Collection.Requests;

namespace Energy.Application.Finance.Collection.Validators;

/// <summary>UpdateCollectionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateCollectionRequestValidator : AbstractValidator<UpdateCollectionRequest>
{
    public UpdateCollectionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PartnerId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.CollectionNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
