using FluentValidation;
using Energy.Shared.Models.V1.Core.SystemSetting.Requests;

namespace Energy.Application.Modules.Core.SystemSetting.Validators;

/// <summary>UpdateSystemSettingRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateSystemSettingRequestValidator : AbstractValidator<UpdateSystemSettingRequest>
{
    public UpdateSystemSettingRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Key).NotEmpty();
    }
}
