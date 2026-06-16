using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalRequest.Validators;

/// <summary>UpdateApprovalRequestRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalRequestRequestValidator : AbstractValidator<UpdateApprovalRequestRequest>
{
    public UpdateApprovalRequestRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalDefinitionVersionId).NotEmpty();
        RuleFor(x => x.RelatedModule).NotEmpty();
        RuleFor(x => x.RelatedEntityType).NotEmpty();
        RuleFor(x => x.RelatedEntityId).NotEmpty();
        RuleFor(x => x.RequestedByUserId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
