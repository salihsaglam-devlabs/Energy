using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.ProgressEntry.Requests;

namespace Energy.Application.FieldOperations.ProgressEntry.Validators;

/// <summary>CreateProgressEntryRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProgressEntryRequestValidator : AbstractValidator<CreateProgressEntryRequest>
{
    public CreateProgressEntryRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
