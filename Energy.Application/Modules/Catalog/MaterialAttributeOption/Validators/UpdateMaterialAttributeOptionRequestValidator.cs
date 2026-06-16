using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeOption.Requests;

namespace Energy.Application.Modules.Catalog.MaterialAttributeOption.Validators;

/// <summary>UpdateMaterialAttributeOptionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMaterialAttributeOptionRequestValidator : AbstractValidator<UpdateMaterialAttributeOptionRequest>
{
    public UpdateMaterialAttributeOptionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.MaterialAttributeDefinitionId).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}
