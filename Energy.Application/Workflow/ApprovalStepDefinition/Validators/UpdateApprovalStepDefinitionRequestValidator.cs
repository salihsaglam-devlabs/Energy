using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalStepDefinition.Requests;

namespace Energy.Application.Workflow.ApprovalStepDefinition.Validators;

/// <summary>UpdateApprovalStepDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalStepDefinitionRequestValidator : AbstractValidator<UpdateApprovalStepDefinitionRequest>
{
    public UpdateApprovalStepDefinitionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalDefinitionVersionId).NotEmpty();
        RuleFor(x => x.ApprovalMode).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
