using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinition.Requests;

namespace Energy.Application.Workflow.ApprovalDefinition.Validators;

/// <summary>CreateApprovalDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalDefinitionRequestValidator : AbstractValidator<CreateApprovalDefinitionRequest>
{
    public CreateApprovalDefinitionRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.RelatedModule).NotEmpty();
        RuleFor(x => x.RelatedEntityType).NotEmpty();
    }
}
