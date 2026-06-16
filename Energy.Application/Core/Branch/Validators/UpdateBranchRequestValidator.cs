using FluentValidation;
using Energy.Shared.Models.V1.Core.Branch.Requests;

namespace Energy.Application.Core.Branch.Validators;

/// <summary>UpdateBranchRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
{
    public UpdateBranchRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
