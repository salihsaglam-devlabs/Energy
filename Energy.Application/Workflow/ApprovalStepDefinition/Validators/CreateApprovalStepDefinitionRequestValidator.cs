using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;

namespace Energy.Application.Workflow.ApprovalStepDefinition.Validators;

/// <summary>CreateApprovalStepDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalStepDefinitionRequestValidator : AbstractValidator<CreateApprovalStepDefinitionRequest>
{
    public CreateApprovalStepDefinitionRequestValidator()
    {
        RuleFor(x => x.ApprovalDefinitionVersionId).NotEmpty();
        RuleFor(x => x.ApprovalMode).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
