using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectPhas.Requests;

namespace Energy.Application.Modules.Projects.ProjectPhas.Validators;

/// <summary>CreateProjectPhasRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProjectPhasRequestValidator : AbstractValidator<CreateProjectPhasRequest>
{
    public CreateProjectPhasRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
