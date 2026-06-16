using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Validators;

/// <summary>UpdateApprovalDelegationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalDelegationRequestValidator : AbstractValidator<UpdateApprovalDelegationRequest>
{
    public UpdateApprovalDelegationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DelegatorUserId).NotEmpty();
        RuleFor(x => x.DelegateUserId).NotEmpty();
    }
}
