using FluentValidation;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;

namespace Energy.Application.Modules.Requests.RequestLine.Validators;

/// <summary>CreateRequestLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateRequestLineRequestValidator : AbstractValidator<CreateRequestLineRequest>
{
    public CreateRequestLineRequestValidator()
    {
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.UnitOfMeasureId).NotEmpty();
    }
}
