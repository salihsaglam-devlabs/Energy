using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalCondition.Validators;

/// <summary>CreateApprovalConditionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalConditionRequestValidator : AbstractValidator<CreateApprovalConditionRequest>
{
    public CreateApprovalConditionRequestValidator()
    {
        RuleFor(x => x.ApprovalDefinitionVersionId).NotEmpty();
        RuleFor(x => x.FieldName).NotEmpty();
        RuleFor(x => x.Operator).NotEmpty();
    }
}
