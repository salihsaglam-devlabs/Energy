using FluentValidation;
using Energy.Shared.Models.V1.Core.Branch.Requests;

namespace Energy.Application.Modules.Core.Branch.Validators;

/// <summary>CreateBranchRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBranchRequestValidator : AbstractValidator<CreateBranchRequest>
{
    public CreateBranchRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
