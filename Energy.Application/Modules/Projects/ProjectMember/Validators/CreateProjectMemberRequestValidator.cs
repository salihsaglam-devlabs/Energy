using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectMember.Requests;

namespace Energy.Application.Modules.Projects.ProjectMember.Validators;

/// <summary>CreateProjectMemberRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProjectMemberRequestValidator : AbstractValidator<CreateProjectMemberRequest>
{
    public CreateProjectMemberRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
