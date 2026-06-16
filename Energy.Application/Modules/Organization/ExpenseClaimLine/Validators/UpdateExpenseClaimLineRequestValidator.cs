using FluentValidation;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;

namespace Energy.Application.Modules.Organization.ExpenseClaimLine.Validators;

/// <summary>UpdateExpenseClaimLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateExpenseClaimLineRequestValidator : AbstractValidator<UpdateExpenseClaimLineRequest>
{
    public UpdateExpenseClaimLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ExpenseClaimId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
