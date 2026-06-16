using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Validators;

/// <summary>UpdateApprovalDefinitionVersionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalDefinitionVersionRequestValidator : AbstractValidator<UpdateApprovalDefinitionVersionRequest>
{
    public UpdateApprovalDefinitionVersionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalDefinitionId).NotEmpty();
    }
}
