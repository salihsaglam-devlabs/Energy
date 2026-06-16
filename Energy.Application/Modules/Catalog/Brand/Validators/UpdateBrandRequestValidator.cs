using FluentValidation;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;

namespace Energy.Application.Modules.Catalog.Brand.Validators;

/// <summary>UpdateBrandRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBrandRequestValidator : AbstractValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
