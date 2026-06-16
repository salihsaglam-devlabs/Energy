using FluentValidation;
using Energy.Shared.Models.V1.Catalog.MaterialAttributeDefinition.Requests;

namespace Energy.Application.Modules.Catalog.MaterialAttributeDefinition.Validators;

/// <summary>CreateMaterialAttributeDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateMaterialAttributeDefinitionRequestValidator : AbstractValidator<CreateMaterialAttributeDefinitionRequest>
{
    public CreateMaterialAttributeDefinitionRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.DataType).NotEmpty();
    }
}
