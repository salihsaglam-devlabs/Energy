using FluentValidation;
using Energy.Shared.Models.V1.Requests.Request.Requests;

namespace Energy.Application.Requests.Request.Validators;

/// <summary>CreateRequestRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateRequestRequestValidator : AbstractValidator<CreateRequestRequest>
{
    public CreateRequestRequestValidator()
    {
        RuleFor(x => x.RequestTypeId).NotEmpty();
        RuleFor(x => x.RequestedByUserId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.RequestNo).NotEmpty();
    }
}
