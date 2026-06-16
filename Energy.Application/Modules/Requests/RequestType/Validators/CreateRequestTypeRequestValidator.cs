using FluentValidation;
using Energy.Shared.Models.V1.Requests.RequestType.Requests;

namespace Energy.Application.Modules.Requests.RequestType.Validators;

/// <summary>CreateRequestTypeRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateRequestTypeRequestValidator : AbstractValidator<CreateRequestTypeRequest>
{
    public CreateRequestTypeRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Category).NotEmpty();
    }
}
