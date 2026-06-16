using FluentValidation;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;

namespace Energy.Application.Core.SystemSetting.Validators;

/// <summary>CreateSystemSettingRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateSystemSettingRequestValidator : AbstractValidator<CreateSystemSettingRequest>
{
    public CreateSystemSettingRequestValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
    }
}
