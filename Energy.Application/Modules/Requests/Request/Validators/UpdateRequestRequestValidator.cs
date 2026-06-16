using FluentValidation;
using Energy.Shared.Models.V1.Requests.Request.Requests;

namespace Energy.Application.Modules.Requests.Request.Validators;

/// <summary>UpdateRequestRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateRequestRequestValidator : AbstractValidator<UpdateRequestRequest>
{
    public UpdateRequestRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RequestTypeId).NotEmpty();
        RuleFor(x => x.RequestedByUserId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.RequestNo).NotEmpty();
    }
}
