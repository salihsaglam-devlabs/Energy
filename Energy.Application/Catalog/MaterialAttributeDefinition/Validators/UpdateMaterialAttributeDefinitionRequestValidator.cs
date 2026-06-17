using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;

namespace Energy.Application.Catalog.MaterialAttributeDefinition.Validators;

/// <summary>UpdateMaterialAttributeDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateMaterialAttributeDefinitionRequestValidator : AbstractValidator<UpdateMaterialAttributeDefinitionRequest>
{
    public UpdateMaterialAttributeDefinitionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DataType).NotEmpty();
    }
}
