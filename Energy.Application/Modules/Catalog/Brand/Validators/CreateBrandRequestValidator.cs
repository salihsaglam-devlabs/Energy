using FluentValidation;
using Energy.Shared.Models.V1.Catalog.Brand.Requests;

namespace Energy.Application.Modules.Catalog.Brand.Validators;

/// <summary>CreateBrandRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBrandRequestValidator : AbstractValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
