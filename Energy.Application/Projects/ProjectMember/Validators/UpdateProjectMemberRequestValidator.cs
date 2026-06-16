using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectMember.Requests;

namespace Energy.Application.Projects.ProjectMember.Validators;

/// <summary>UpdateProjectMemberRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProjectMemberRequestValidator : AbstractValidator<UpdateProjectMemberRequest>
{
    public UpdateProjectMemberRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
