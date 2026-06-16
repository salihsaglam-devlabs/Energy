using FluentValidation;
using Energy.Shared.Models.V1.Projects.ProjectNote.Requests;

namespace Energy.Application.Projects.ProjectNote.Validators;

/// <summary>CreateProjectNoteRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProjectNoteRequestValidator : AbstractValidator<CreateProjectNoteRequest>
{
    public CreateProjectNoteRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}
