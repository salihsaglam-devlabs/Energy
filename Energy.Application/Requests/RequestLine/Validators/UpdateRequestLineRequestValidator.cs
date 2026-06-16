using FluentValidation;
using Energy.Shared.Models.V1.Requests.RequestLine.Requests;

namespace Energy.Application.Requests.RequestLine.Validators;

/// <summary>UpdateRequestLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateRequestLineRequestValidator : AbstractValidator<UpdateRequestLineRequest>
{
    public UpdateRequestLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequestId).NotEmpty();
        RuleFor(x => x.UnitOfMeasureId).NotEmpty();
    }
}
